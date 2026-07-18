using EfCoreInterceptors.Domain.Abstractions;

namespace EfCoreInterceptors.Domain.Events;

internal sealed record BlogDeletedEvent(Guid BlogId, string Title) : IDomainEvent;
