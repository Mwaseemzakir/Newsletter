using Microsoft.EntityFrameworkCore;
using VectorSearchRag.Domain.Entities;

namespace VectorSearchRag.Infrastructure.Data;

public sealed class AppDbContext : DbContext
{
    public const int EmbeddingDimensions = 768;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Blog> Blogs => Set<Blog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Enables the pgvector extension when the database is created/migrated.
        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.Entity<Blog>(builder =>
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Title).HasMaxLength(300).IsRequired();
            builder.Property(b => b.Description).IsRequired();

            // Maps the CLR Vector to a Postgres `vector(768)` column.
            builder.Property(b => b.Embedding)
                .HasColumnType($"vector({EmbeddingDimensions})");
        });
    }
}
