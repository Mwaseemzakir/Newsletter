# cURL requests — Client IP Safelist

Base URL: `http://localhost:5225` (HTTPS: `https://localhost:7055`). Adjust to match `Properties/launchSettings.json`.

Access is restricted to the IP addresses configured in the safelist (`appsettings.json`).
Requests from an IP that is not on the list are rejected (401/403).

> Import any command below into Postman via **Import → Raw text** (paste the cURL).

### Get weather forecast
```bash
curl -i "http://localhost:5225/WeatherForecast"
```
