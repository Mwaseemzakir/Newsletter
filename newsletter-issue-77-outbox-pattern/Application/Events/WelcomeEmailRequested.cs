namespace OutboxPattern.Application.Events;

/// <summary>
/// The side effect we want to happen after a user registers. The handler serializes this
/// into an <see cref="Domain.Entities.OutboxMessage"/> instead of sending the email inline,
/// so the email can never be lost if the request dies after the save.
///
/// This is a Wolverine message contract (routed to <c>WelcomeEmailHandler</c>), so it is
/// <c>public</c> — Wolverine's generated dispatch code must be able to see it.
/// </summary>
public sealed record WelcomeEmailRequested(string Email);
