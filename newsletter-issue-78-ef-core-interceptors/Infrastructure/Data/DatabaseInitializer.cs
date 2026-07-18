using Microsoft.EntityFrameworkCore;

namespace EfCoreInterceptors.Infrastructure.Data;

/// <summary>
/// On startup: applies migrations so the Blogs and OutboxMessages tables exist.
/// Seeding is intentionally left to the API so you can watch the interceptors fire.
/// </summary>
internal static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync(cancellationToken);
    }
}
