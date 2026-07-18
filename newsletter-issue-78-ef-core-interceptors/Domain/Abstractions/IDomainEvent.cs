namespace EfCoreInterceptors.Domain.Abstractions;

/// <summary>
/// Marker for a domain event raised by an entity. The
/// <see cref="Interceptors.OutboxInterceptor"/> serializes these into the outbox
/// table inside the same transaction as the change that produced them.
/// </summary>
public interface IDomainEvent;