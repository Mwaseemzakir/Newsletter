namespace EfCoreInterceptors.Application.Models;

/// <summary>The payload for creating a blog — the handler only ever sets the title.</summary>
public sealed record CreateBlogRequest(string Title);
