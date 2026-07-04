using System.Linq.Expressions;
using VectorSearchRag.Domain.Entities;
using VectorSearchRag.Application.Models;

namespace VectorSearchRag.Application.Mappers;

/// <summary>Maps <see cref="Blog"/> entities to their API response shapes.</summary>
internal static class BlogMapper
{
    /// <summary>
    /// EF projection used inside <c>.Select(...)</c> so the query only reads the
    /// columns we expose and never loads the large embedding vector.
    /// </summary>
    public static readonly Expression<Func<Blog, BlogResponse>> ToResponse =
        blog => new BlogResponse(blog.Id, blog.Title, blog.Description);

    /// <summary>In-memory mapping for already-materialized blogs plus their distance.</summary>
    public static BlogMatchResponse ToMatchResponse(this Blog blog, double distance) =>
        new(blog.Id, blog.Title, blog.Description, distance);
}
