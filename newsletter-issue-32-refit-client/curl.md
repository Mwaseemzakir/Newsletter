# cURL requests — Refit client

Base URL: `http://localhost:5105` (HTTPS: `https://localhost:7018`). Adjust to match `RefitClient/Properties/launchSettings.json`.

This is the API that the `ConsoleApp` Refit client consumes. You can also call it directly:

> Import any command below into Postman via **Import → Raw text** (paste the cURL).

### Get all users
```bash
curl "http://localhost:5105/api/user/"
```

### Add a user
```bash
curl -X POST "http://localhost:5105/api/user/" \
  -H "Content-Type: application/json" \
  -d '{
    "id": "f8c3de3d-1fea-4d7c-a8b0-29f63c4c3454",
    "name": "Muhammad Waseem"
  }'
```
