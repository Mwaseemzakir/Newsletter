# cURL requests — In-Memory Caching

Base URL: `http://localhost:5206` (HTTPS: `https://localhost:7262`). Adjust to match `Properties/launchSettings.json`.

> Import any command below into Postman via **Import → Raw text** (paste the cURL).

### Check whether a number is prime (cached after first call)
```bash
curl "http://localhost:5206/PrimeNumbers/97"
```

### Call again with the same number to hit the cache
```bash
curl "http://localhost:5206/PrimeNumbers/97"
```

### Try a non-prime number
```bash
curl "http://localhost:5206/PrimeNumbers/100"
```
