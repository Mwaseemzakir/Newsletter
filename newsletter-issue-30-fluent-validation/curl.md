# cURL requests — FluentValidation

Base URL: `http://localhost:5110` (HTTPS: `https://localhost:7021`). Adjust to match `Properties/launchSettings.json`.

> Import any command below into Postman via **Import → Raw text** (paste the cURL).

### Get all students
```bash
curl "http://localhost:5110/student"
```

### Add a valid student
```bash
curl -X POST "http://localhost:5110/student" \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "age": 22,
    "email": "waseem@example.com",
    "firstName": "Muhammad",
    "lastName": "Waseem",
    "phoneNumber": "+1234567890"
  }'
```

### Add an invalid student (triggers validation errors)
```bash
curl -X POST "http://localhost:5110/student" \
  -H "Content-Type: application/json" \
  -d '{
    "id": 0,
    "age": 0,
    "email": "not-an-email",
    "firstName": "",
    "lastName": "",
    "phoneNumber": ""
  }'
```

### Get the sample weather forecast
```bash
curl "http://localhost:5110/WeatherForecast"
```
