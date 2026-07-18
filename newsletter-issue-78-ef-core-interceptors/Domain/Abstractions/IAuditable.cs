namespace EfCoreInterceptors.Domain.Abstractions;

/// <summary>
/// Marker interface for entities that track who created/updated them and when.
/// The <see cref="Interceptors.AuditInterceptor"/> stamps these automatically.
/// </summary>
internal interface IAuditable
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    string CreatedBy { get; set; }
    string? UpdatedBy { get; set; }
}
