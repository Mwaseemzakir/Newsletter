using Pgvector;

namespace VectorSearchRag.Domain.Entities;

public sealed class Blog
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    // The semantic embedding of "Title + Description".
    // 768 dimensions to match Gemini's text-embedding-004 model.
    // Stored in PostgreSQL as a `vector(768)` column via pgvector.
    public Vector? Embedding { get; set; }
}
