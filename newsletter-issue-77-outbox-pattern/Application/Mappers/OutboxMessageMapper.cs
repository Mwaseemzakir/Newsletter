using OutboxPattern.Application.Models;
using OutboxPattern.Domain.Entities;

namespace OutboxPattern.Application.Mappers;

/// <summary>Maps <see cref="OutboxMessage"/> rows to their API response shape.</summary>
internal static class OutboxMessageMapper
{
    public static OutboxMessageResponse ToResponse(this OutboxMessage message) => new(
        message.Id, message.Type, message.Payload, message.CreatedAt,
        message.ProcessedAt, message.Error, message.RetryCount);
}
