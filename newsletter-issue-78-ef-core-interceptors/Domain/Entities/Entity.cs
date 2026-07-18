using EfCoreInterceptors.Domain.Abstractions;

namespace EfCoreInterceptors.Domain.Entities;

/// <summary>
/// Base class for entities that raise domain events. Events accumulate in memory
/// and are drained by the <see cref="Interceptors.OutboxInterceptor"/> on save.
/// </summary>
public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

    protected void Raise(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() =>
        _domainEvents.Clear();
}
