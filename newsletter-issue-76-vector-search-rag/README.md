# Issue 76 — AI Search in .NET using PostgreSQL + Vector Search + RAG

A runnable ASP.NET Core demo that compares **normal keyword search** with
**semantic vector search**, and adds a **RAG** (Retrieval-Augmented Generation)
endpoint on top.

It uses a single `Blog` entity, seeds **100 dummy blogs**, and exposes search APIs.

## What it shows

| Endpoint | What it does |
| --- | --- |
| `GET /api/blogs` | List all 100 seeded blogs |
| `GET /api/blogs/search/normal?q=` | Classic SQL `ILIKE` / `Contains` search |
| `GET /api/blogs/search/vector?q=&take=5` | Semantic search by cosine distance over embeddings |
| `GET /api/blogs/ask?q=&take=5` | RAG: retrieve top blogs, then have an LLM answer using only them |
| `GET /api/blogs/info` | Which providers are active (Gemini vs local fallback) |

The classic example: search **`secure my APIs`**.
- `search/normal` returns `[]` — no blog *contains* those words.
- `search/vector` returns **"JWT Authentication in ASP.NET Core"** first — it matches *meaning*.

## Architecture

```
Question
   ↓
Embedding (Gemini text-embedding-004, or local fallback)
   ↓
Vector search in PostgreSQL (pgvector, cosine distance)
   ↓
Top N relevant blogs
   ↓
LLM (Gemini gemini-2.0-flash) — "answer using ONLY these blogs"
   ↓
Grounded answer
```

## Tech

- ASP.NET Core (.NET 9) Web API
- PostgreSQL + [`pgvector`](https://github.com/pgvector/pgvector)
- EF Core with `Npgsql` + `Pgvector.EntityFrameworkCore`
- Gemini Embeddings + Generation API (optional — falls back to an offline provider)

## Embedding providers (Gemini with local fallback)

- **Gemini** is used when `Gemini:ApiKey` is configured. It produces real 768-dim
  semantic embeddings (`text-embedding-004`) and grounded answers (`gemini-2.0-flash`).
- **Local fallback** is used automatically when no key is set. It is a dependency-free
  hashing embedder so the demo runs fully offline. It only captures *word overlap*,
  not true meaning, so vector results are approximate — but the headline example
  ("secure my APIs" → JWT) still works.

Get a free key at https://aistudio.google.com/app/apikey and set it via user secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "Gemini:ApiKey" "YOUR_KEY"
```

## Running locally

### 1. Use your locally installed PostgreSQL

This demo talks to a PostgreSQL installed on your machine (default port **5432**,
user `postgres` / password `postgres`, database `blogsdb`) — see the connection
string in `appsettings.json`. No Docker required.

If you don't have PostgreSQL yet, install it from
https://www.postgresql.org/download/windows/ and create the database:

```bash
createdb -U postgres blogsdb
```

Because this demo uses vector search, the **`pgvector`** extension must be
available in that PostgreSQL install. On Windows you can add it via
**StackBuilder** or a prebuilt binary from https://github.com/pgvector/pgvector.
The EF migration runs `CREATE EXTENSION vector` on startup, so you only need the
extension files present — no manual SQL.

### 2. Run the API

```bash
dotnet run
```

On startup the app applies the EF migration (which enables the `vector` extension
and creates the `Blogs` table), then seeds 100 blogs with embeddings.
Swagger opens at the root.

### 3. Try it

```bash
curl "http://localhost:5076/api/blogs/search/normal?q=secure my APIs"   # -> []
curl "http://localhost:5076/api/blogs/search/vector?q=secure my APIs"   # -> JWT Authentication, ...
curl "http://localhost:5076/api/blogs/ask?q=how do I protect my endpoints"
```

## The Blog entity

```csharp
public class Blog
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Vector? Embedding { get; set; }   // vector(768) via pgvector
}
```

## Bonus: Supabase

Supabase is managed PostgreSQL with `pgvector` built in. The code here works against
it unchanged — just point the `Postgres` connection string at your Supabase database.
