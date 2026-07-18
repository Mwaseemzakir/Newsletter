using System.Text.Json;
using EfCoreInterceptors.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EfCoreInterceptors.Infrastructure.Interceptors;

/// <summary>
/// Drains domain events off every tracked <see cref="Entity"/> and writes them as
/// <see cref="OutboxMessage"/> rows in the SAME SaveChanges/transaction. No handler
/// ever inserts an outbox row by hand.
/// </summary>
internal sealed class OutboxInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        if (eventData.Context is null)
            return base.SavingChangesAsync(eventData, result, ct);

        var outboxMessages = eventData.Context.ChangeTracker
            .Entries<Entity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = domainEvent.GetType().AssemblyQualifiedName!,
                Payload = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
                CreatedAt = DateTime.UtcNow
            })
            .ToList();

        eventData.Context.Set<OutboxMessage>().AddRange(outboxMessages);

        foreach (var entry in eventData.Context.ChangeTracker.Entries<Entity>())
            entry.Entity.ClearDomainEvents();

        return base.SavingChangesAsync(eventData, result, ct);
    }
}
