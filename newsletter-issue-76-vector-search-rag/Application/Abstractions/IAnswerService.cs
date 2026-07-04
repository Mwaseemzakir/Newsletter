using VectorSearchRag.Domain.Entities;

namespace VectorSearchRag.Application.Abstractions;

/// <summary>
/// The "Generation" half of RAG: given a user question and the blogs we retrieved,
/// produce a natural-language answer grounded in those blogs.
/// </summary>
public interface IAnswerService
{
    string ProviderName { get; }

    Task<string> AnswerAsync(string question, IReadOnlyList<Blog> context, CancellationToken cancellationToken = default);
}
