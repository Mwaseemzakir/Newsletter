using Microsoft.EntityFrameworkCore;
using OutboxPattern.Application.Abstractions;
using OutboxPattern.Infrastructure.BackgroundJobs;
using OutboxPattern.Infrastructure.Data;
using OutboxPattern.Infrastructure.Services;
using Quartz;

namespace OutboxPattern.Extensions;

/// <summary>
/// Service-registration extensions so <c>Program.cs</c> stays a thin composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>Registers the PostgreSQL <see cref="AppDbContext"/>.</summary>
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres")
            ?? throw new InvalidOperationException("Connection string 'Postgres' is missing.");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        return services;
    }

    /// <summary>
    /// Registers the email sender and the Quartz job that drains the outbox every
    /// 10 seconds. Quartz creates a DI scope per fire, so the job can inject scoped services.
    /// (Wolverine itself is configured on the host in <c>Program.cs</c>.)
    /// </summary>
    public static IServiceCollection AddOutboxProcessing(this IServiceCollection services)
    {
        services.AddScoped<IEmailSender, LoggingEmailSender>();

        services.AddQuartz(configurator =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configurator
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(trigger => trigger
                    .ForJob(jobKey)
                    .WithSimpleSchedule(schedule => schedule
                        .WithIntervalInSeconds(10)
                        .RepeatForever()));
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        return services;
    }
}
