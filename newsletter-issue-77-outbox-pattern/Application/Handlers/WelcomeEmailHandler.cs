using OutboxPattern.Application.Abstractions;
using OutboxPattern.Application.Events;

namespace OutboxPattern.Application.Handlers;

/// <summary>
/// Wolverine handler for <see cref="WelcomeEmailRequested"/>. Wolverine discovers it by
/// convention (a public <c>*Handler</c> with a <c>Handle</c> method), so dispatch is
/// open for extension: a new event just needs its own handler — no central switch to edit.
///
/// Single responsibility: translate the message into an email send, delegating the actual
/// work to <see cref="IEmailSender"/> (method injection, resolved per message by Wolverine).
/// </summary>
public sealed class WelcomeEmailHandler
{
    public Task Handle(WelcomeEmailRequested message, IEmailSender emailSender, CancellationToken cancellationToken) =>
        emailSender.SendWelcomeEmailAsync(message.Email, cancellationToken);
}
