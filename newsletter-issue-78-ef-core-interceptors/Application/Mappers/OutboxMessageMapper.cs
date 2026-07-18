using EfCoreInterceptors.Domain.Entities;
using EfCoreInterceptors.Application.Models;

namespace EfCoreInterceptors.Application.Mappers;

/// <summary>Maps <see cref="OutboxMessage"/> rows to their API response shape.</summary>
internal static class OutboxMessageMapper
{
    public static OutboxMessageResponse ToResponse(this OutboxMessage message) => new(
        message.Id, message.Type, message.Payload, message.CreatedAt, message.ProcessedAt);
}
