using EfCoreInterceptors.Domain.Abstractions;
using EfCoreInterceptors.Domain.Events;

namespace EfCoreInterceptors.Domain.Entities;

/// <summary>
/// A single entity that participates in all three interceptors:
///   - <see cref="ISoftDeletable"/>  -> SoftDeleteInterceptor flips IsDeleted instead of deleting the row
///   - <see cref="IAuditable"/>       -> AuditInterceptor stamps Created/Updated By + At
///   - inherits <see cref="Entity"/>  -> OutboxInterceptor drains its domain events into the outbox
///
/// Notice the handlers (see BlogsController) never touch any of these fields directly.
/// </summary>
public sealed class Blog : Entity, ISoftDeletable, IAuditable
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;

    // ISoftDeletable — set by the interceptor, never by handlers.
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    // IAuditable — set by the interceptor, never by handlers.
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    private Blog() { } // EF

    /// <summary>Factory that raises a <see cref="BlogCreatedEvent"/> for the outbox.</summary>
    public static Blog Create(string title)
    {
        var blog = new Blog
        {
            Id = Guid.NewGuid(),
            Title = title
        };

        blog.Raise(new BlogCreatedEvent(blog.Id, blog.Title));
        return blog;
    }

    /// <summary>
    /// Raises a <see cref="BlogDeletedEvent"/>. The actual soft delete is performed by the
    /// SoftDeleteInterceptor when the handler calls <c>context.Remove(blog)</c>.
    /// </summary>
    public void Delete() =>
        Raise(new BlogDeletedEvent(Id, Title));
}
