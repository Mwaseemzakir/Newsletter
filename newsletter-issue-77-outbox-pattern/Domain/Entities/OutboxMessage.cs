namespace OutboxPattern.Domain.Entities;

/// <summary>
/// One pending side effect, written in the SAME transaction as the business data that
/// produced it. The <see cref="BackgroundJobs.ProcessOutboxMessagesJob"/> later reads
/// the unprocessed rows (<see cref="ProcessedAt"/> is null), dispatches them, and stamps
/// <see cref="ProcessedAt"/>. Failures land in <see cref="Error"/> and bump
/// <see cref="RetryCount"/> so the row is retried on the next poll.
/// </summary>
public sealed class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? Error { get; set; }
    public int RetryCount { get; set; }
}
