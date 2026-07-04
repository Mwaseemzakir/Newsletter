using Microsoft.EntityFrameworkCore;
using VectorSearchRag.Application.Abstractions;

namespace VectorSearchRag.Infrastructure.Data;

/// <summary>
/// On startup: ensures the database/schema exist, then seeds 100 dummy blogs
/// (each with a generated embedding) if the table is empty.
/// </summary>
internal static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var sp = scope.ServiceProvider;

        var context = sp.GetRequiredService<AppDbContext>();
        var embedding = sp.GetRequiredService<IEmbeddingService>();
        var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseInitializer");

        // Apply migrations so the pgvector extension and tables are created.
        await context.Database.MigrateAsync(cancellationToken);

        if (await context.Blogs.AnyAsync(cancellationToken))
        {
            logger.LogInformation("Blogs already seeded. Skipping.");
            return;
        }

        logger.LogInformation("Seeding 100 dummy blogs using embedding provider: {Provider}", embedding.ProviderName);

        var blogs = DummyBlogs.Generate();
        foreach (var blog in blogs)
        {
            // Embed "Title + Description" so both contribute to the meaning.
            blog.Embedding = await embedding.GenerateAsync($"{blog.Title}. {blog.Description}", cancellationToken);
        }

        context.Blogs.AddRange(blogs);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Seeded {Count} blogs.", blogs.Count);
    }
}
