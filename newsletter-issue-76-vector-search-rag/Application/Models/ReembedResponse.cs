namespace VectorSearchRag.Application.Models;

/// <summary>Result of re-embedding all blogs.</summary>
public sealed record ReembedResponse(int BlogsReembedded, string EmbeddingProvider);
