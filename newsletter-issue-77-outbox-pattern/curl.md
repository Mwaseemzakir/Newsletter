# cURL requests — The Outbox Pattern

Base URL: `http://localhost:5077` (HTTPS: `https://localhost:7077`). Adjust to match `Properties/launchSettings.json`.

Requires a locally installed PostgreSQL running on the default port `5432` (see this project's README).

> Import any command below into Postman via **Import → Raw text** (paste the cURL).

### Register a user (writes the user + a WelcomeEmailRequested outbox row in one transaction)
```bash
curl -X POST "http://localhost:5077/api/users" \
  -H "Content-Type: application/json" \
  -d '{"email":"alice@demo.dev"}'
```

### Register another
```bash
curl -X POST "http://localhost:5077/api/users" \
  -H "Content-Type: application/json" \
  -d '{"email":"bob@demo.dev"}'
```

### List users
```bash
curl "http://localhost:5077/api/users"
```

### Outbox messages
Right after a register, `ProcessedAt` is `null`. Within ~10 seconds the background job
dispatches the event (watch the console log the welcome email) and stamps `ProcessedAt`.

```bash
curl "http://localhost:5077/api/outbox"
```
