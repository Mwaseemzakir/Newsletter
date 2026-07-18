# cURL requests — EF Core Interceptors

Base URL: `http://localhost:5078` (HTTPS: `https://localhost:7078`). Adjust to match `Properties/launchSettings.json`.

Requires a locally installed PostgreSQL running on the default port `5432` (see this project's README).

> Import any command below into Postman via **Import → Raw text** (paste the cURL).

### Create a blog (audited as alice, raises BlogCreatedEvent)
```bash
curl -X POST "http://localhost:5078/api/blogs" \
  -H "Content-Type: application/json" \
  -H "X-User: alice@demo.dev" \
  -d '{"title":"Interceptors in EF Core"}'
```

### Create another (no header -> falls back to the demo user)
```bash
curl -X POST "http://localhost:5078/api/blogs" \
  -H "Content-Type: application/json" \
  -d '{"title":"Soft Delete Done Right"}'
```

### List blogs (soft-deleted rows are hidden by the query filter)
```bash
curl "http://localhost:5078/api/blogs"
```

### Soft delete a blog (replace {id}; audited as carol, raises BlogDeletedEvent)
```bash
curl -X DELETE "http://localhost:5078/api/blogs/{id}" \
  -H "X-User: carol@demo.dev"
```

### List INCLUDING deleted (proves the row is still there, IsDeleted=true)
```bash
curl "http://localhost:5078/api/blogs/with-deleted"
```

### Outbox messages (one row per domain event, written by the interceptor)
```bash
curl "http://localhost:5078/api/outbox"
```
