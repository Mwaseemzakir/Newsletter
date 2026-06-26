# cURL requests — Delegating (HTTP message) Handler

Base URL: `http://localhost:5283` (HTTPS: `https://localhost:7086`). Adjust to match `Properties/launchSettings.json`.

Calling this endpoint makes an outgoing HTTP call that flows through the `LoggingHandler`
delegating handler — watch the application console for the logged request/response.

> Import the command below into Postman via **Import → Raw text** (paste the cURL).

### Get dog facts
```bash
curl "http://localhost:5283/Dogs"
```
