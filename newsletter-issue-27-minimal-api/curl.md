# cURL requests — Minimal APIs

Base URL: `http://localhost:5244` (HTTPS: `https://localhost:7152`). Adjust to match `Properties/launchSettings.json`.

This project shows the same operations as both **minimal API** endpoints and **controller** endpoints. All values are passed as query parameters.

> Import any command below into Postman via **Import → Raw text** (paste the cURL).

## Minimal API endpoints

### Get user
```bash
curl "http://localhost:5244/GetUser"
```

### Create a user
```bash
curl -X POST "http://localhost:5244/CreateUser?RollNo=1&UserName=Waseem"
```

### Update a user
```bash
curl -X PUT "http://localhost:5244/UpdateUser?RollNo=1&UserName=Ali"
```

### Delete a user
```bash
curl -X DELETE "http://localhost:5244/DeleteUser?RollNo=1"
```

### Get paginated users
```bash
curl "http://localhost:5244/GetUsers?page=1&pageSize=10"
```

## Controller endpoints

### Get users
```bash
curl "http://localhost:5244/Users"
```

### Create a user
```bash
curl -X POST "http://localhost:5244/Users?rollNo=1&userName=Waseem"
```

### Update a user
```bash
curl -X PUT "http://localhost:5244/Users?rollNo=1&userName=Ali"
```

### Delete a user
```bash
curl -X DELETE "http://localhost:5244/Users?rollNo=1"
```
