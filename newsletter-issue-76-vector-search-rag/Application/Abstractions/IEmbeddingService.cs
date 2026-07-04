using Pgvector;

namespace VectorSearchRag.Application.Abstractions;

/// <summary>
/// Turns text into a numeric vector that captures its meaning.
/// </summary>
public interface IEmbeddingService
{
    /// <summary>The provider that is actually being used (Gemini or Local fallback).</summary>
    string ProviderName { get; }

    Task<Vector> GenerateAsync(string text, CancellationToken cancellationToken = default);
}
