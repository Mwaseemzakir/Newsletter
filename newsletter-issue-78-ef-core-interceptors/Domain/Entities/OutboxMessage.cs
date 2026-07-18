namespace EfCoreInterceptors.Domain.Entities;

/// <summary>
/// One serialized domain event, written by the OutboxInterceptor in the same
/// transaction as the change that produced it. A background processor (Wolverine,
/// a hosted service, etc.) would later read unprocessed rows and dispatch them.
/// </summary>
public sealed class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}
