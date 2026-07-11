namespace OutboxPattern.Application.Models;

/// <summary>The payload for registering a user — the handler only ever sets the email.</summary>
public sealed record RegisterUserRequest(string Email);
