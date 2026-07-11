using Microsoft.EntityFrameworkCore;
using OutboxPattern.Domain.Entities;

namespace OutboxPattern.Infrastructure.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email).HasMaxLength(256).IsRequired();
            builder.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<OutboxMessage>(builder =>
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Type).HasMaxLength(200).IsRequired();
            builder.Property(o => o.Payload).IsRequired();

            // The processor scans for unprocessed rows oldest-first — index that path.
            builder.HasIndex(o => o.ProcessedAt);
        });
    }
}
