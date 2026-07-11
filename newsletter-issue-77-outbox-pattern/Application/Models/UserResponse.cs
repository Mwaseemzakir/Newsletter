namespace OutboxPattern.Application.Models;

/// <summary>A registered user as returned by the API.</summary>
public sealed record UserResponse(
    Guid Id,
    string Email,
    DateTime CreatedAt);
