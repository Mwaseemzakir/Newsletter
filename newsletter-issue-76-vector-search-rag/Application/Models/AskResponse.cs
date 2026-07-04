namespace VectorSearchRag.Application.Models;

public sealed record AskResponse(string Question, string Answer, string LlmProvider, IReadOnlyList<BlogMatchResponse> Sources);
