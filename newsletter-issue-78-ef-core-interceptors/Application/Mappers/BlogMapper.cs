using EfCoreInterceptors.Domain.Entities;
using EfCoreInterceptors.Application.Models;

namespace EfCoreInterceptors.Application.Mappers;

/// <summary>Maps <see cref="Blog"/> entities to their API response shape.</summary>
internal static class BlogMapper
{
    public static BlogResponse ToResponse(this Blog blog) => new(
        blog.Id, blog.Title, blog.IsDeleted, blog.DeletedAt,
        blog.CreatedAt, blog.UpdatedAt, blog.CreatedBy, blog.UpdatedBy);
}
