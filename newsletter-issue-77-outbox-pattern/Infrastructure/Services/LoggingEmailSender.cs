using OutboxPattern.Application.Abstractions;

namespace OutboxPattern.Infrastructure.Services;

/// <summary>
/// Stand-in email sender that just logs. In production this is where SendGrid/SMTP/etc.
/// would go — and nothing else in the app changes, because callers depend on
/// <see cref="IEmailSender"/>, not this class.
/// </summary>
public sealed class LoggingEmailSender(ILogger<LoggingEmailSender> logger) : IEmailSender
{
    public Task SendWelcomeEmailAsync(string email, CancellationToken cancellationToken)
    {
        logger.LogInformation("Sending welcome email to {Email}", email);
        return Task.CompletedTask;
    }
}
