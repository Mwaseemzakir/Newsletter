# cURL requests — API Key Authentication

Base URL: `http://localhost:5111` (HTTPS: `https://localhost:7055`). Adjust to match `Properties/launchSettings.json`.

The protected endpoint requires an `ApiKey` request header whose value matches the `ApiKey`
in `appsettings.json` (default below). Without it you get `401 Unauthorized`.

> Import any command below into Postman via **Import → Raw text** (paste the cURL).

### GET /Student with a valid API key
```bash
curl "http://localhost:5111/Student" \
  -H "ApiKey: 3DBF7CF8-64CA-43C5-8840-9EA60E5A5A75"
```

### GET /Student without the key (returns 401)
```bash
curl -i "http://localhost:5111/Student"
```

### GET /Student/{id} (this action is not protected by the filter)
```bash
curl "http://localhost:5111/Student/5"
```

### POST /Student
```bash
curl -X POST "http://localhost:5111/Student" \
  -H "ApiKey: 3DBF7CF8-64CA-43C5-8840-9EA60E5A5A75"
```

### PUT /Student/{id}
```bash
curl -X PUT "http://localhost:5111/Student/5" \
  -H "ApiKey: 3DBF7CF8-64CA-43C5-8840-9EA60E5A5A75"
```
