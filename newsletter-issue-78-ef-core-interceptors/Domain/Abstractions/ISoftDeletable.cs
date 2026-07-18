namespace EfCoreInterceptors.Domain.Abstractions;

/// <summary>
/// Marker interface for entities that should be soft-deleted instead of removed.
/// The <see cref="Interceptors.SoftDeleteInterceptor"/> watches for these.
/// </summary>
internal interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
}
