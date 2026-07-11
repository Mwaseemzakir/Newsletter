using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OutboxPattern.Infrastructure.Data;
using Quartz;
using Wolverine;

namespace OutboxPattern.Infrastructure.BackgroundJobs;

/// <summary>
/// Quartz job that drains the OutboxMessages table. Quartz fires it every 10 seconds (see
/// <see cref="Extensions.DependencyInjection.AddOutboxProcessing"/>) and creates a fresh DI
/// scope per run, so the <see cref="AppDbContext"/> and publisher are injected directly.
/// Each run dispatches up to 20 unprocessed rows (oldest first) and marks them processed;
/// anything that throws is recorded in <c>Error</c> with <c>RetryCount</c> bumped, so the
/// row is picked up again on the next fire.
///
/// The fetch uses PostgreSQL's <c>FOR UPDATE SKIP LOCKED</c> inside a transaction, so if
/// several instances of the app run at once each row is processed by exactly one of them.
/// <see cref="DisallowConcurrentExecutionAttribute"/> stops a slow run from overlapping the
/// next tick.
/// </summary>
[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob(AppDbContext dbContext, IMessageBus messageBus) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var cancellationToken = context.CancellationToken;

        // FOR UPDATE SKIP LOCKED: lock the rows we grab so a second instance skips them.
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var messages = await dbContext.OutboxMessages
            .FromSqlRaw(
                """
                SELECT "Id", "Type", "Payload", "CreatedAt", "ProcessedAt", "Error", "RetryCount"
                FROM "OutboxMessages"
                WHERE "ProcessedAt" IS NULL
                ORDER BY "CreatedAt"
                LIMIT 20
                FOR UPDATE SKIP LOCKED
                """)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                var eventType = Type.GetType(message.Type)
                    ?? throw new InvalidOperationException($"Unknown event type '{message.Type}'.");

                var @event = JsonSerializer.Deserialize(message.Payload, eventType)
                    ?? throw new InvalidOperationException($"Could not deserialize payload for '{message.Type}'.");

                // InvokeAsync (not PublishAsync): run the handler inline and await it, so the
                // row is only marked processed once the side effect actually succeeded. A
                // throw is caught below, leaving ProcessedAt null for the next fire to retry.
                await messageBus.InvokeAsync(@event, cancellationToken);

                message.ProcessedAt = DateTime.UtcNow;
                message.Error = null;
            }
            catch (Exception ex)
            {
                message.Error = ex.Message;
                message.RetryCount++;
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}
