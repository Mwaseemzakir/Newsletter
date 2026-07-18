using EfCoreInterceptors.Domain.Abstractions;
using EfCoreInterceptors.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EfCoreInterceptors.Infrastructure.Interceptors;

/// <summary>
/// Stamps <see cref="IAuditable"/> entities with who/when on insert and update.
/// A soft delete arrives here as a <see cref="EntityState.Modified"/> entry (the
/// SoftDeleteInterceptor converted it), so deletes get audited too.
/// </summary>
internal sealed class AuditInterceptor(ICurrentUserService currentUser) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        if (eventData.Context is null)
            return base.SavingChangesAsync(eventData, result, ct);

        var now = DateTime.UtcNow;
        var user = currentUser.UserId;

        foreach (var entry in eventData.Context.ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = user;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = user;
            }
        }

        return base.SavingChangesAsync(eventData, result, ct);
    }
}
