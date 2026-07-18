# Issue 78 — EF Core Interceptors: Soft Delete, Audit & Outbox

A runnable ASP.NET Core demo of the three interceptors from the newsletter, all wired
into a **single** `Blog` entity so one request can show all of them firing at once.

The whole point: the controller only ever sets a `Title` and calls `Remove`. Every
cross-cutting concern — soft delete, audit columns, outbox rows — is handled by
`SaveChangesInterceptor`s the handlers know nothing about.

> ⚠️ **Demo only.** The business logic lives directly in the controller to keep the
> sample in one place and easy to follow. In a real application a controller should only
> receive the request, delegate the work to a service/handler, and return the response —
> don't put business logic in controllers.

## What it shows

| Endpoint | What it does |
| --- | --- |
| `POST /api/blogs` | Create a blog. AuditInterceptor stamps `CreatedAt/CreatedBy`; OutboxInterceptor writes a `BlogCreatedEvent` row. |
| `DELETE /api/blogs/{id}` | "Delete" a blog. SoftDeleteInterceptor flips `IsDeleted` instead of removing it; AuditInterceptor stamps `UpdatedBy`; OutboxInterceptor writes a `BlogDeletedEvent`. |
| `GET /api/blogs` | List blogs. Soft-deleted rows are hidden by a global query filter. |
| `GET /api/blogs/with-deleted` | Same query with `IgnoreQueryFilters()` — proves the "deleted" row is still in the table with `IsDeleted=true`. |
| `GET /api/outbox` | The outbox rows the OutboxInterceptor wrote, one per domain event. |

> Send an optional `X-User` header on writes to watch different values land in the
> `CreatedBy` / `UpdatedBy` columns. With no header it falls back to a fixed demo user.

## The three interceptors

| File | Hooks on | Effect |
| --- | --- | --- |
| [`Infrastructure/Interceptors/SoftDeleteInterceptor.cs`](./Infrastructure/Interceptors/SoftDeleteInterceptor.cs) | `EntityState.Deleted` + `ISoftDeletable` | Converts the entry to `Modified` and sets `IsDeleted`/`DeletedAt`. |
| [`Infrastructure/Interceptors/AuditInterceptor.cs`](./Infrastructure/Interceptors/AuditInterceptor.cs) | `Added` / `Modified` + `IAuditable` | Sets `CreatedAt/By` on insert, `UpdatedAt/By` on update. |
| [`Infrastructure/Interceptors/OutboxInterceptor.cs`](./Infrastructure/Interceptors/OutboxInterceptor.cs) | any tracked `Entity` with domain events | Serializes each event into an `OutboxMessage` row in the same transaction. |

Because soft delete rewrites the entry from `Deleted` to `Modified` *before* the audit
interceptor runs, a delete is also audited (`UpdatedBy` = whoever deleted it).

## How they're registered

Interceptors with dependencies (the `AuditInterceptor` needs `ICurrentUserService`) are
registered through DI and resolved when the `DbContext` is built. To keep `Program.cs`
a thin composition root, the wiring lives in
[`Extensions/DependencyInjection.cs`](./Extensions/DependencyInjection.cs)
(`Program.cs` just calls `builder.Services.AddInterceptors().AddDatabase(...)`):

```csharp
public static IServiceCollection AddInterceptors(this IServiceCollection services)
{
    services.AddHttpContextAccessor();
    services.AddScoped<ICurrentUserService, CurrentUserService>();

    services.AddScoped<SoftDeleteInterceptor>();
    services.AddScoped<AuditInterceptor>();
    services.AddScoped<OutboxInterceptor>();

    return services;
}

public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("Postgres")!;

    services.AddDbContext<AppDbContext>((sp, options) =>
        options.UseNpgsql(connectionString)
               .AddInterceptors(
                   sp.GetRequiredService<SoftDeleteInterceptor>(),
                   sp.GetRequiredService<AuditInterceptor>(),
                   sp.GetRequiredService<OutboxInterceptor>()));

    return services;
}
```

## Tech

- ASP.NET Core (.NET 9) Web API
- PostgreSQL
- EF Core 9 with `Npgsql`

## Running locally

### 1. Use your locally installed PostgreSQL

This demo talks to a PostgreSQL installed on your machine (default port **5432**,
user `postgres` / password `postgres`, database `interceptorsdb`) — see the
connection string in `appsettings.json`. No Docker required.

If you don't have PostgreSQL yet, install it from
https://www.postgresql.org/download/windows/ and create the database:

```bash
createdb -U postgres interceptorsdb
```

### 2. Run the API

```bash
dotnet run
```

On startup the app applies the EF migration (creating the `Blogs` and `OutboxMessages`
tables). Swagger opens at the root.

### 3. Try it

```bash
# create two blogs (note the X-User header on the first)
curl -X POST http://localhost:5078/api/blogs -H "Content-Type: application/json" -H "X-User: alice@demo.dev" -d '{"title":"Interceptors in EF Core"}'
curl -X POST http://localhost:5078/api/blogs -H "Content-Type: application/json" -d '{"title":"Soft Delete Done Right"}'

# delete the first one (replace {id}) — watch it become a soft delete
curl -X DELETE "http://localhost:5078/api/blogs/{id}" -H "X-User: carol@demo.dev"

curl http://localhost:5078/api/blogs               # deleted row is hidden
curl http://localhost:5078/api/blogs/with-deleted  # deleted row still there, IsDeleted=true
curl http://localhost:5078/api/outbox              # BlogCreatedEvent x2 + BlogDeletedEvent
```

See [`curl.md`](./curl.md) for the full set of ready-to-paste requests.

## The Blog entity

One entity implements all three contracts, but the handlers never touch the
interceptor-owned fields directly:

```csharp
public class Blog : Entity, ISoftDeletable, IAuditable
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    // ISoftDeletable — set by SoftDeleteInterceptor
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    // IAuditable — set by AuditInterceptor
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}
```

## Going further

Once outbox rows exist, something has to dispatch them. The newsletter uses
[Wolverine](https://wolverinefx.net/) as a mediator that picks up outbox messages and
publishes them reliably — drop it in and point it at the `OutboxMessages` table.
