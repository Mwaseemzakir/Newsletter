namespace EfCoreInterceptors.Application.Abstractions;

/// <summary>Supplies the current user id to the AuditInterceptor.</summary>
internal interface ICurrentUserService
{
    string UserId { get; }
}
