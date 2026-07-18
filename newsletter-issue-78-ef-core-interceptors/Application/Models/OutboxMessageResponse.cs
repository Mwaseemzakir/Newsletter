namespace EfCoreInterceptors.Application.Models;

/// <summary>An outbox row written by the OutboxInterceptor.</summary>
public sealed record OutboxMessageResponse(
    Guid Id,
    string Type,
    string Payload,
    DateTime CreatedAt,
    DateTime? ProcessedAt);
