# cURL requests — MailKit (sending emails)

Base URL: `http://localhost:5074` (HTTPS: `https://localhost:7285`). Adjust the port to match `Properties/launchSettings.json`.

> Import any command below into Postman via **Import → Raw text** (paste the cURL).

### Send an email
Configure your SMTP settings in `appsettings.json` before calling this.

```bash
curl -X POST "http://localhost:5074/Email" \
  -H "Content-Type: application/json" \
  -d '{
    "to": "recipient@example.com",
    "subject": "Hello from MailKit",
    "plainText": "This is the plain-text body.",
    "html": "<h1>This is the HTML body.</h1>"
  }'
```

### Get the sample weather forecast
```bash
curl "http://localhost:5074/WeatherForecast"
```
