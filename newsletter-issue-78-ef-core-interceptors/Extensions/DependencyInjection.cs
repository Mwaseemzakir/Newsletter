using EfCoreInterceptors.Application.Abstractions;
using EfCoreInterceptors.Infrastructure.Data;
using EfCoreInterceptors.Infrastructure.Interceptors;
using EfCoreInterceptors.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace EfCoreInterceptors.Extensions;

/// <summary>
/// Service-registration extensions so <c>Program.cs</c> stays a thin, readable
/// composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    /// Registers the current-user service and the three SaveChanges interceptors
    /// (AuditInterceptor depends on <see cref="ICurrentUserService"/>), all through DI
    /// so they can be resolved when the <see cref="AppDbContext"/> is built.
    /// </summary>
    public static IServiceCollection AddInterceptors(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<SoftDeleteInterceptor>();
        services.AddScoped<AuditInterceptor>();
        services.AddScoped<OutboxInterceptor>();

        return services;
    }

    /// <summary>Registers the PostgreSQL <see cref="AppDbContext"/> with the interceptors attached.</summary>
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres")
            ?? throw new InvalidOperationException("Connection string 'Postgres' is missing.");

        services.AddDbContext<AppDbContext>((sp, options) =>
            options.UseNpgsql(connectionString)
                   .AddInterceptors(
                       sp.GetRequiredService<SoftDeleteInterceptor>(),
                       sp.GetRequiredService<AuditInterceptor>(),
                       sp.GetRequiredService<OutboxInterceptor>()));

        return services;
    }
}
