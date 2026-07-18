using EfCoreInterceptors.Domain.Abstractions;

namespace EfCoreInterceptors.Domain.Events;

internal sealed record BlogCreatedEvent(Guid BlogId, string Title) : IDomainEvent;
