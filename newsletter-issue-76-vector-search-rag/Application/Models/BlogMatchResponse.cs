namespace VectorSearchRag.Application.Models;

/// <summary>A blog plus how close it was to the query (lower distance = more similar).</summary>
public sealed record BlogMatchResponse(Guid Id, string Title, string Description, double Distance);
