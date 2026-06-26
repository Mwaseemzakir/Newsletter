# cURL requests — Security Headers

Base URL: `http://localhost:5118` (HTTPS: `https://localhost:7227`). Adjust to match `Properties/launchSettings.json`.

> Import any command below into Postman via **Import → Raw text** (paste the cURL).

### Get weather forecast and inspect the security headers
Use `-i` (or `-v`) to print the response headers added by the middleware
(e.g. `X-Content-Type-Options`, `X-Frame-Options`, `Content-Security-Policy`).

```bash
curl -i "http://localhost:5118/WeatherForecast"
```
