using Microsoft.EntityFrameworkCore;

namespace OutboxPattern.Infrastructure.Data;

/// <summary>
/// On startup: applies migrations so the Users and OutboxMessages tables exist.
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
