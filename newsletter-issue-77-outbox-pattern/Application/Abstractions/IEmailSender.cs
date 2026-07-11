namespace OutboxPattern.Application.Abstractions;

/// <summary>
/// Sends emails. The handler depends on this abstraction, not a concrete provider (DIP), so
/// swapping the logging stub for SendGrid/SMTP/etc. is a one-line DI change with no handler
/// edits. Public because Wolverine's generated handler code resolves it from the container.
/// </summary>
public interface IEmailSender
{
    Task SendWelcomeEmailAsync(string email, CancellationToken cancellationToken);
}
