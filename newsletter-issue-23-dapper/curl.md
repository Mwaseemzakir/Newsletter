# cURL requests — Dapper CRUD API

Base URL: `http://localhost:5131` (HTTPS: `https://localhost:7216`). Adjust to match `Properties/launchSettings.json`.

Run `script.sql` and update the connection string in `appsettings.json` before calling these.

> Import any command below into Postman via **Import → Raw text** (paste the cURL).

### Get all newsletters
```bash
curl "http://localhost:5131/Newsletter"
```

### Get a newsletter by id
```bash
curl "http://localhost:5131/Newsletter/1"
```

### Create a newsletter
```bash
curl -X POST "http://localhost:5131/Newsletter" \
  -H "Content-Type: application/json" \
  -d '{
    "type": "Tech",
    "name": ".NET Weekly",
    "url": "https://mwaseemzakir.substack.com",
    "about": "Weekly .NET tips and tutorials."
  }'
```

### Update a newsletter
```bash
curl -X PUT "http://localhost:5131/Newsletter/1" \
  -H "Content-Type: application/json" \
  -d '{
    "type": "Tech",
    "name": ".NET Weekly (updated)",
    "url": "https://mwaseemzakir.substack.com",
    "about": "Updated description."
  }'
```

### Delete a newsletter
```bash
curl -X DELETE "http://localhost:5131/Newsletter/1"
```

### Get the sample weather forecast
```bash
curl "http://localhost:5131/WeatherForecast"
```
