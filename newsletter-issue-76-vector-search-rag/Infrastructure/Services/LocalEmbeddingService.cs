using Pgvector;
using VectorSearchRag.Infrastructure.Data;
using VectorSearchRag.Application.Abstractions;

namespace VectorSearchRag.Infrastructure.Services;

/// <summary>
/// A dependency-free, offline embedding generator used when no Gemini API key
/// is configured. It hashes each word into a fixed set of buckets and L2-normalizes
/// the result, so texts that share words land close together under cosine distance.
///
/// This is NOT a real semantic model — it only captures word overlap, not meaning.
/// It exists so the demo runs end-to-end without any external dependency.
/// Configure "Gemini:ApiKey" to get real semantic search.
/// </summary>
internal sealed class LocalEmbeddingService : IEmbeddingService
{
    private const int Dimensions = AppDbContext.EmbeddingDimensions;

    private static readonly char[] Separators =
        [' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?', '(', ')', '[', ']', '"', '\'', '-', '/'];

    public string ProviderName => "Local hashing fallback (no API key — word-overlap only)";

    public Task<Vector> GenerateAsync(string text, CancellationToken cancellationToken = default)
    {
        var vector = new float[Dimensions];

        foreach (var token in Tokenize(text))
        {
            // Two independent hashes per token: one picks the bucket, one picks the sign.
            // This is a tiny "feature hashing" trick (a.k.a. the hashing trick).
            var hash = StableHash(token);
            var bucket = (int)(hash % (uint)Dimensions);
            var sign = (hash & 1) == 0 ? 1f : -1f;
            vector[bucket] += sign;
        }

        Normalize(vector);
        return Task.FromResult(new Vector(vector));
    }

    private static IEnumerable<string> Tokenize(string text) =>
        text.ToLowerInvariant()
            .Split(Separators, StringSplitOptions.RemoveEmptyEntries)
            .Where(t => t.Length > 1);

    private static void Normalize(float[] vector)
    {
        double sumOfSquares = 0;
        foreach (var v in vector)
        {
            sumOfSquares += v * (double)v;
        }

        var magnitude = (float)Math.Sqrt(sumOfSquares);
        if (magnitude == 0)
        {
            return;
        }

        for (var i = 0; i < vector.Length; i++)
        {
            vector[i] /= magnitude;
        }
    }

    // FNV-1a: a small, stable, framework-independent hash (string.GetHashCode is randomized per run).
    private static uint StableHash(string value)
    {
        const uint offset = 2166136261;
        const uint prime = 16777619;

        var hash = offset;
        foreach (var c in value)
        {
            hash ^= c;
            hash *= prime;
        }

        return hash;
    }
}
