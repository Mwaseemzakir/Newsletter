using EfCoreInterceptors.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EfCoreInterceptors.Infrastructure.Interceptors;

/// <summary>
/// Turns a hard delete into a soft delete. When a handler calls <c>context.Remove(blog)</c>,
/// EF marks the entry as <see cref="EntityState.Deleted"/>; we intercept that, flip the entry
/// back to <see cref="EntityState.Modified"/>, and set the soft-delete flags instead.
/// </summary>
internal sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        if(eventData.Context is null)
            return base.SavingChangesAsync(eventData, result, ct);

        foreach(var entry in eventData.Context.ChangeTracker.Entries())
        {
            if(entry is { State: EntityState.Deleted, Entity: ISoftDeletable entity })
            {
                entry.State = EntityState.Modified;
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.UtcNow;
            }
        }

        return base.SavingChangesAsync(eventData, result, ct);
    }
}
