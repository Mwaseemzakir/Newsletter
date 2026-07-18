using EfCoreInterceptors.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCoreInterceptors.Infrastructure.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(builder =>
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Title).HasMaxLength(300).IsRequired();
            builder.Property(b => b.CreatedBy).HasMaxLength(200).IsRequired();
            builder.Property(b => b.UpdatedBy).HasMaxLength(200);

            // Domain events are an in-memory concern only — never a column.
            builder.Ignore(b => b.DomainEvents);

            // Soft-deleted rows stay in the table but disappear from normal queries.
            // Use IgnoreQueryFilters() to see them (see GET /api/blogs/with-deleted).
            builder.HasQueryFilter(b => !b.IsDeleted);
        });

        modelBuilder.Entity<OutboxMessage>(builder =>
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Type).HasMaxLength(200).IsRequired();
            builder.Property(o => o.Payload).IsRequired();
        });
    }
}
