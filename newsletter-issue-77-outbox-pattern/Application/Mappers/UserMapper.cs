using OutboxPattern.Application.Models;
using OutboxPattern.Domain.Entities;

namespace OutboxPattern.Application.Mappers;

/// <summary>Maps <see cref="User"/> entities to their API response shape.</summary>
internal static class UserMapper
{
    public static UserResponse ToResponse(this User user) => new(
        user.Id, user.Email, user.CreatedAt);
}
