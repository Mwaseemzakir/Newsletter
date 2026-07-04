using System.Text;
using VectorSearchRag.Domain.Entities;
using VectorSearchRag.Application.Abstractions;

namespace VectorSearchRag.Infrastructure.Services;

/// <summary>
/// Offline fallback for the "Generation" step. It does not call an LLM; it simply
/// stitches together the retrieved blogs so the /ask endpoint still returns something
/// useful when no Gemini API key is configured.
/// </summary>
internal sealed class LocalAnswerService : IAnswerService
{
    public string ProviderName => "Local fallback (no LLM — retrieved blogs only)";

    public Task<string> AnswerAsync(
        string question, IReadOnlyList<Blog> context, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"No LLM is configured, so here are the most relevant blogs for: \"{question}\"");
        sb.AppendLine();

        if (context.Count == 0)
        {
            sb.AppendLine("No matching blogs were found.");
        }
        else
        {
            foreach (var blog in context)
            {
                sb.AppendLine($"• {blog.Title}");
            }

            sb.AppendLine();
            sb.AppendLine("Set \"Gemini:ApiKey\" in configuration to get a generated, RAG-style answer.");
        }

        return Task.FromResult(sb.ToString().TrimEnd());
    }
}
