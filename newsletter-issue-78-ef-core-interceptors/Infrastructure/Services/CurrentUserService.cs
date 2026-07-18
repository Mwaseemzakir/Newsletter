using EfCoreInterceptors.Application.Abstractions;

namespace EfCoreInterceptors.Infrastructure.Services;

/// <summary>
/// Demo implementation. In a real app you would read the authenticated principal
/// from <c>IHttpContextAccessor</c>; here we honour an optional <c>X-User</c> header
/// so you can see different values land in the audit columns, and fall back to a
/// fixed demo user otherwise.
/// </summary>
internal sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private const string DefaultUser = "demo-user@newsletter.dev";

    public string UserId { get; } = Resolve(httpContextAccessor);

    private static string Resolve(IHttpContextAccessor httpContextAccessor)
    {
        var header = httpContextAccessor.HttpContext?.Request.Headers["X-User"].ToString();
        return string.IsNullOrWhiteSpace(header) ? DefaultUser : header!;
    }
}
