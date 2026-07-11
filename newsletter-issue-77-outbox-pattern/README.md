# Issue 77 — The Outbox Pattern in ASP.NET Core

A runnable ASP.NET Core demo of the **Outbox Pattern**: instead of firing a side effect
(send the welcome email) right after the save, the handler writes the side effect to a
database table **in the same transaction** as the business data. A background job picks it
up and processes it reliably — so the email can never be silently lost if the request
crashes after the save.

> This is the deliberately **simple** version: no EF interceptor, no domain events, no
> mediator. The handler writes the outbox row by hand so the whole pattern fits in one
> method. (The interceptor-driven version is [issue 78](../newsletter-issue-78-ef-core-interceptors).)

> ⚠️ **Demo only.** The business logic lives directly in the controller to keep the
> sample in one place and easy to follow. In a real application a controller should only
> receive the request, delegate the work to a service/handler, and return the response —
> don't put business logic in controllers.

## What it shows

| Endpoint | What it does |
| --- | --- |
| `POST /api/users` | Register a user. Writes the `User` **and** a `WelcomeEmailRequested` outbox row in one `SaveChangesAsync`. No email is sent here. |
| `GET /api/users` | List registered users. |
| `GET /api/outbox` | The outbox rows. Right after a register `ProcessedAt` is `null`; within ~10s the background job stamps it. |

## How it works

### 1. The handler writes to the outbox, not to the email service

```csharp
var user = User.Create(request.Email);

var outboxMessage = new OutboxMessage
{
    Id = Guid.NewGuid(),
    Type = typeof(WelcomeEmailRequested).AssemblyQualifiedName!, // full assembly info so Type.GetType can rehydrate it
    Payload = JsonSerializer.Serialize(new WelcomeEmailRequested(user.Email)),
    CreatedAt = DateTime.UtcNow
};

context.Users.Add(user);
context.OutboxMessages.Add(outboxMessage);

await context.SaveChangesAsync(ct); // user + message commit atomically
```

If the save rolls back, the outbox message rolls back with it. If it succeeds, the
message is guaranteed to be there for the processor to pick up.

### 2. A Quartz job drains the outbox

[`ProcessOutboxMessagesJob`](./Infrastructure/BackgroundJobs/ProcessOutboxMessagesJob.cs)
is a Quartz `IJob` that [`DependencyInjection`](./Extensions/DependencyInjection.cs) wires
to a trigger firing every 10 seconds. Quartz creates a fresh DI scope per fire (so the job
injects a scoped `AppDbContext`), and `[DisallowConcurrentExecution]` stops a slow run from
overlapping the next tick. Each run:

1. Fetches up to 20 unprocessed rows, **oldest first**.
2. Deserializes each payload back into its event type.
3. Publishes it through **Wolverine** (`IMessageBus.InvokeAsync`), which routes the message
   to its handler — e.g. [`WelcomeEmailHandler`](./Application/Handlers/WelcomeEmailHandler.cs),
   which calls `IEmailSender`. No central `switch`: a new event just needs a new handler.
4. Stamps `ProcessedAt`. Anything that throws is recorded in `Error` with `RetryCount`
   bumped, so the row is retried next tick.

> `InvokeAsync` runs the handler inline and awaits it, so a row is only marked processed
> once the side effect actually succeeded — keeping the outbox guarantee intact. (Adopt
> Wolverine's own durable inbox if you'd rather hand off with `PublishAsync`.)

### 3. `FOR UPDATE SKIP LOCKED` makes it safe to run more than one instance

The fetch runs inside a transaction using PostgreSQL's `FOR UPDATE SKIP LOCKED`, so if you
run several copies of the app each row is processed by exactly one of them:

```sql
SELECT "Id", "Type", "Payload", "CreatedAt", "ProcessedAt", "Error", "RetryCount"
FROM "OutboxMessages"
WHERE "ProcessedAt" IS NULL
ORDER BY "CreatedAt"
LIMIT 20
FOR UPDATE SKIP LOCKED
```

## Tech

- ASP.NET Core (.NET 9) Web API
- PostgreSQL
- EF Core 9 with `Npgsql`
- [Quartz.NET](https://www.quartz-scheduler.net/) for the scheduled outbox processor
- [Wolverine](https://wolverinefx.net/) as the pub/sub layer routing events to handlers

## Running locally

### 1. Use your locally installed PostgreSQL

This demo talks to a PostgreSQL installed on your machine (default port **5432**,
user `postgres` / password `postgres`, database `outboxdb`) — see the connection string in
`appsettings.json`. No Docker required.

If you don't have PostgreSQL yet, install it from
https://www.postgresql.org/download/windows/ and create the database:

```bash
createdb -U postgres outboxdb
```

### 2. Run the API

```bash
dotnet run
```

On startup the app applies the EF migration (creating the `Users` and `OutboxMessages`
tables). Swagger opens at the root.

### 3. Try it

```bash
# register a user — the welcome email is NOT sent inline
curl -X POST http://localhost:5077/api/users -H "Content-Type: application/json" -d '{"email":"alice@demo.dev"}'

curl http://localhost:5077/api/outbox   # ProcessedAt is null...
# wait ~10 seconds, watch the console log "Sending welcome email to alice@demo.dev"
curl http://localhost:5077/api/outbox   # ...now ProcessedAt is stamped
```

See [`curl.md`](./curl.md) for the full set of ready-to-paste requests.

## Going further

Wolverine already routes each event to its handler. To make the side effect real, swap the
logging [`LoggingEmailSender`](./Infrastructure/Services/LoggingEmailSender.cs) for a
SendGrid/SMTP implementation of `IEmailSender` — one line in
[`DependencyInjection`](./Extensions/DependencyInjection.cs), no handler changes. New side
effects are just new events + handlers. The pattern stays the same: **write the message in
the same transaction, process it asynchronously.**
