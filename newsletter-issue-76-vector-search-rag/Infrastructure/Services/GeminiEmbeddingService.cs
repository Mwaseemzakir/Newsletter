using Pgvector;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using VectorSearchRag.Infrastructure.Data;
using VectorSearchRag.Application.Abstractions;

namespace VectorSearchRag.Infrastructure.Services;

/// <summary>
/// Generates embeddings using Google's Gemini gemini-embedding-001 model
/// (text-embedding-004 has been retired).
/// Requires a free API key from https://aistudio.google.com/app/apikey
/// set in configuration as "Gemini:ApiKey".
/// </summary>
internal sealed class GeminiEmbeddingService(HttpClient httpClient, string apiKey) : IEmbeddingService
{
    private const string Model = "models/gemini-embedding-001";
    private const string Endpoint =
        "https://generativelanguage.googleapis.com/v1beta/models/gemini-embedding-001:embedContent";

    public string ProviderName => "Gemini (gemini-embedding-001)";

    public async Task<Vector> GenerateAsync(string text, CancellationToken cancellationToken = default)
    {
        var requestBody = new GeminiRequest
        {
            Model = Model,
            Content = new GeminiContent
            {
                Parts = [new GeminiPart { Text = text }]
            },
            // gemini-embedding-001 defaults to 3072 dims; request 768 to match our vector column.
            OutputDimensionality = AppDbContext.EmbeddingDimensions
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, $"{Endpoint}?key={apiKey}")
        {
            Content = new StringContent(
                JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
        };

        using var response = await httpClient.SendAsync(request, cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        if(!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Gemini embedding request failed ({(int)response.StatusCode}): {json}");
        }

        var parsed = JsonSerializer.Deserialize<GeminiResponse>(json);
        var values = parsed?.Embedding?.Values
            ?? throw new InvalidOperationException("Gemini returned an empty embedding.");

        if(values.Length != AppDbContext.EmbeddingDimensions)
        {
            throw new InvalidOperationException(
                $"Expected {AppDbContext.EmbeddingDimensions} dimensions but Gemini returned {values.Length}.");
        }

        // gemini-embedding-001 only returns a unit-length vector at its full 3072 dims.
        // Since we request 768, the result is un-normalized — normalize so cosine distance is correct.
        Normalize(values);

        return new Vector(values);
    }

    private static void Normalize(float[] vector)
    {
        double sumOfSquares = 0;
        foreach (var v in vector)
            sumOfSquares += v * (double)v;

        var magnitude = Math.Sqrt(sumOfSquares);
        if(magnitude < double.Epsilon)
            return;

        for(var i = 0; i < vector.Length; i++)
            vector[i] = (float)(vector[i] / magnitude);
    }

    private sealed class GeminiRequest
    {
        [JsonPropertyName("model")] public string Model { get; set; } = string.Empty;
        [JsonPropertyName("content")] public GeminiContent Content { get; set; } = new();
        [JsonPropertyName("outputDimensionality")] public int OutputDimensionality { get; set; }
    }

    private sealed class GeminiContent
    {
        [JsonPropertyName("parts")] public GeminiPart[] Parts { get; set; } = [];
    }

    private sealed class GeminiPart
    {
        [JsonPropertyName("text")] public string Text { get; set; } = string.Empty;
    }

    private sealed class GeminiResponse
    {
        [JsonPropertyName("embedding")] public GeminiEmbedding? Embedding { get; set; }
    }

    private sealed class GeminiEmbedding
    {
        [JsonPropertyName("values")] public float[]? Values { get; set; }
    }
}
