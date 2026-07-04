# cURL requests — Vector Search + RAG

Base URL: `http://localhost:5076` (HTTPS: `https://localhost:7076`). Adjust to match `Properties/launchSettings.json`.

Requires a running PostgreSQL with the `pgvector` extension (see this project's README). Query
parameters containing spaces are URL-encoded below (`%20`).

> Import any command below into Postman via **Import → Raw text** (paste the cURL).

### Which providers are active (Gemini vs local fallback)
```bash
curl "http://localhost:5076/api/blogs/info"
```

### Browse all blogs
```bash
curl "http://localhost:5076/api/blogs"
```

### NORMAL search (SQL ILIKE) — returns nothing for this query
```bash
curl "http://localhost:5076/api/blogs/search/normal?q=secure%20my%20APIs"
```

### VECTOR search — surfaces semantically related blogs
```bash
curl "http://localhost:5076/api/blogs/search/vector?q=secure%20my%20APIs&take=5"
```

### RAG — answer the question using the retrieved blogs
```bash
curl "http://localhost:5076/api/blogs/ask?q=How%20do%20I%20secure%20my%20APIs%3F&take=5"
```
