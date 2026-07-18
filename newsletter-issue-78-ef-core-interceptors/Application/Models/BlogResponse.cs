namespace EfCoreInterceptors.Application.Models;

/// <summary>A blog with the audit + soft-delete columns the interceptors populate.</summary>
public sealed record BlogResponse(
    Guid Id,
    string Title,
    bool IsDeleted,
    DateTime? DeletedAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string CreatedBy,
    string? UpdatedBy);
