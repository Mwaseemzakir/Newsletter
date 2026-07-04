using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using VectorSearchRag.Infrastructure.Data;
using VectorSearchRag.Application.Mappers;
using VectorSearchRag.Application.Models;
using VectorSearchRag.Application.Abstractions;

namespace VectorSearchRag.Controllers;

[ApiController]
[Route("api/blogs")]
public sealed class BlogsController(AppDbContext context, IEmbeddingService embedding, IAnswerService answer) : ControllerBase
{
    /// <summary>Which providers are currently active (Gemini or local fallback).</summary>
    [HttpGet("info")]
    public ActionResult<ProviderInfoResponse> Info() =>
        Ok(new ProviderInfoResponse(embedding.ProviderName, answer.ProviderName));

    /// <summary>
    /// Re-generates the embedding for every blog using the currently active provider,
    /// embedding "Title + Description" (the same text used when seeding).
    /// Run this after switching embedding providers (e.g. local fallback -> Gemini)
    /// so the stored vectors match what search queries are embedded with.
    /// </summary>
    [HttpPost("reembed")]
    public async Task<ActionResult<ReembedResponse>> Reembed(CancellationToken ct)
    {
        var blogs = await context.Blogs.ToListAsync(ct);

        foreach (var blog in blogs)
        {
            // Embed "Title + Description" so both contribute to the meaning,
            // identical to how blogs are embedded during seeding.
            blog.Embedding = await embedding.GenerateAsync($"{blog.Title}. {blog.Description}", ct);
        }

        await context.SaveChangesAsync(ct);

        return Ok(new ReembedResponse(blogs.Count, embedding.ProviderName));
    }

    /// <summary>All blogs (without embeddings), for browsing.</summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BlogResponse>>> GetAll(CancellationToken ct)
    {
        var blogs = await context.Blogs
            .OrderBy(b => b.Title)
            .Select(BlogMapper.ToResponse)
            .ToListAsync(ct);

        return Ok(blogs);
    }

    /// <summary>
    /// NORMAL search: classic SQL LIKE / Contains matching.
    /// Try "secure my APIs" — it returns nothing, because no title/description contains those exact words.
    /// </summary>
    [HttpGet("search/normal")]
    public async Task<ActionResult<IReadOnlyList<BlogResponse>>> NormalSearch(
        [FromQuery] string q, CancellationToken ct)
    {
        if(string.IsNullOrWhiteSpace(q))
        {
            return BadRequest("Query 'q' is required.");
        }

        var blogs = await context.Blogs
            .Where(b => EF.Functions.ILike(b.Title, $"%{q}%")
                     || EF.Functions.ILike(b.Description, $"%{q}%"))
            .Select(BlogMapper.ToResponse)
            .ToListAsync(ct);

        return Ok(blogs);
    }

    /// <summary>
    /// VECTOR search: embeds the query and orders blogs by cosine distance.
    /// Try "secure my APIs" — it surfaces "JWT Authentication", "API Key Authentication", etc.,
    /// even though none of those share the query's words.
    /// </summary>
    [HttpGet("search/vector")]
    public async Task<ActionResult<IReadOnlyList<BlogMatchResponse>>> VectorSearch(
        [FromQuery] string q, [FromQuery] int take = 5, CancellationToken ct = default)
    {
        if(string.IsNullOrWhiteSpace(q))
        {
            return BadRequest("Query 'q' is required.");
        }

        take = Math.Clamp(take, 1, 50);

        var queryEmbedding = await embedding.GenerateAsync(q, ct);

        var matches = await context.Blogs
            .Where(b => b.Embedding != null)
            .OrderBy(b => b.Embedding!.CosineDistance(queryEmbedding))
            .Take(take)
            .Select(b => new BlogMatchResponse(
                b.Id, b.Title, b.Description,
                b.Embedding!.CosineDistance(queryEmbedding)))
            .ToListAsync(ct);

        return Ok(matches);
    }

    /// <summary>
    /// RAG: retrieve the most relevant blogs by vector search, then ask the LLM
    /// to answer the question using ONLY those blogs.
    /// </summary>
    [HttpGet("ask")]
    public async Task<ActionResult<AskResponse>> Ask(
        [FromQuery] string q, [FromQuery] int take = 5, CancellationToken ct = default)
    {
        if(string.IsNullOrWhiteSpace(q))
        {
            return BadRequest("Query 'q' is required.");
        }

        take = Math.Clamp(take, 1, 10);
        var queryEmbedding = await embedding.GenerateAsync(q, ct);

        // Retrieval step.
        var retrieved = await context.Blogs
            .Where(b => b.Embedding != null)
            .OrderBy(b => b.Embedding!.CosineDistance(queryEmbedding))
            .Take(take)
            .Select(b => new
            {
                Blog = b,
                Distance = b.Embedding!.CosineDistance(queryEmbedding)
            })
            .ToListAsync(ct);

        var blogs = retrieved.Select(r => r.Blog).ToList();

        // Generation step.
        var generatedAnswer = await answer.AnswerAsync(q, blogs, ct);

        var sources = retrieved
            .Select(r => r.Blog.ToMatchResponse(r.Distance))
            .ToList();

        return Ok(new AskResponse(q, generatedAnswer, answer.ProviderName, sources));
    }
}
