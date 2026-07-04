using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using VectorSearchRag.Domain.Entities;
using VectorSearchRag.Application.Abstractions;

namespace VectorSearchRag.Infrastructure.Services;

/// <summary>
/// Generates a grounded answer using Gemini's generateContent API.
/// The prompt instructs the model to answer ONLY from the retrieved blogs.
/// </summary>
internal sealed class GeminiAnswerService(HttpClient httpClient, string apiKey) : IAnswerService
{
    private const string Endpoint =
        "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

    public string ProviderName => "Gemini (gemini-2.0-flash)";

    public async Task<string> AnswerAsync(
        string question, IReadOnlyList<Blog> context, CancellationToken cancellationToken = default)
    {
        var prompt = BuildPrompt(question, context);

        var requestBody = new GenerateRequest
        {
            Contents = [new Content { Parts = [new Part { Text = prompt }] }]
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, $"{Endpoint}?key={apiKey}")
        {
            Content = new StringContent(
                JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
        };

        using var response = await httpClient.SendAsync(request, cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Gemini generateContent failed ({(int)response.StatusCode}): {json}");
        }

        var parsed = JsonSerializer.Deserialize<GenerateResponse>(json);
        return parsed?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text?.Trim()
            ?? "The model did not return an answer.";
    }

    internal static string BuildPrompt(string question, IReadOnlyList<Blog> context)
    {
        var sb = new StringBuilder();
        sb.AppendLine("You are a helpful assistant for a software blog.");
        sb.AppendLine("Answer the user's question using ONLY the blog posts provided below.");
        sb.AppendLine("If the blogs do not contain the answer, say you don't have enough information.");
        sb.AppendLine("Cite the blog titles you used.");
        sb.AppendLine();
        sb.AppendLine("=== BLOG POSTS ===");
        for (var i = 0; i < context.Count; i++)
        {
            sb.AppendLine($"[{i + 1}] {context[i].Title}");
            sb.AppendLine(context[i].Description);
            sb.AppendLine();
        }

        sb.AppendLine("=== QUESTION ===");
        sb.AppendLine(question);
        return sb.ToString();
    }

    private sealed class GenerateRequest
    {
        [JsonPropertyName("contents")] public Content[] Contents { get; set; } = [];
    }

    private sealed class Content
    {
        [JsonPropertyName("parts")] public Part[] Parts { get; set; } = [];
    }

    private sealed class Part
    {
        [JsonPropertyName("text")] public string Text { get; set; } = string.Empty;
    }

    private sealed class GenerateResponse
    {
        [JsonPropertyName("candidates")] public Candidate[]? Candidates { get; set; }
    }

    private sealed class Candidate
    {
        [JsonPropertyName("content")] public Content? Content { get; set; }
    }
}
