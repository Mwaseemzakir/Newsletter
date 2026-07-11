namespace OutboxPattern.Application.Models;

/// <summary>An outbox row, so you can watch it move from unprocessed to processed.</summary>
public sealed record OutboxMessageResponse(
    Guid Id,
    string Type,
    string Payload,
    DateTime CreatedAt,
    DateTime? ProcessedAt,
    string? Error,
    int RetryCount);
