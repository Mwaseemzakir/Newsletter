using VectorSearchRag.Domain.Entities;

namespace VectorSearchRag.Infrastructure.Data;

/// <summary>
/// Produces 100 dummy blog posts about .NET / software topics.
/// Titles are intentionally written with domain jargon (e.g. "JWT Authentication")
/// so you can see how vector search matches plain-English queries like "secure my APIs".
/// </summary>
internal static class DummyBlogs
{
    public static IReadOnlyList<Blog> Generate()
    {
        var blogs = new List<Blog>();

        foreach (var (title, description) in Posts)
        {
            blogs.Add(new Blog
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description
            });
        }

        return blogs;
    }

    // 100 curated posts: (Title, Description).
    private static readonly (string Title, string Description)[] Posts =
    [
        ("JWT Authentication in ASP.NET Core", "Learn how to secure your Web APIs using JSON Web Tokens, validate signing keys, and protect endpoints with the [Authorize] attribute."),
        ("API Key Authentication in .NET", "Protect your minimal APIs and controllers by validating API keys sent in request headers using a custom authorization filter."),
        ("Refresh Tokens and Token Rotation", "Keep users signed in securely by issuing short-lived access tokens and rotating long-lived refresh tokens safely."),
        ("Role-Based Authorization in ASP.NET Core", "Restrict access to endpoints based on user roles and claims, and learn the difference between authentication and authorization."),
        ("Securing APIs with OAuth 2.0 and OpenID Connect", "Understand the authorization code flow, scopes, and how to integrate an identity provider with your .NET backend."),
        ("Rate Limiting in ASP.NET Core", "Protect your services from abuse by throttling requests per client using the built-in rate limiting middleware."),
        ("CORS Explained for .NET Developers", "Configure cross-origin resource sharing correctly so your frontend can safely call your backend APIs."),
        ("Validating Requests with FluentValidation", "Keep your controllers clean by moving model validation rules into strongly typed validator classes."),
        ("Global Exception Handling in ASP.NET Core", "Use middleware and IExceptionHandler to return consistent error responses across your entire API."),
        ("Minimal APIs vs Controllers", "Compare the two ways of building HTTP endpoints in .NET and learn when each approach fits best."),
        ("Dependency Injection Deep Dive", "Understand service lifetimes - transient, scoped, and singleton - and avoid common captive dependency bugs."),
        ("Entity Framework Core Migrations", "Evolve your database schema safely using code-first migrations, and learn how to roll changes back."),
        ("Dapper for High-Performance Data Access", "Run raw SQL with a lightweight micro-ORM when you need maximum speed and full control over queries."),
        ("Connection Pooling in Npgsql", "See how PostgreSQL connections are reused under the hood and how to tune the pool for throughput."),
        ("Caching with IMemoryCache", "Speed up your application by storing frequently accessed data in memory with sensible expiration policies."),
        ("Distributed Caching with Redis", "Share cached data across multiple instances of your app using Redis and IDistributedCache."),
        ("Background Jobs with Hosted Services", "Run recurring or long-running work in the background using IHostedService and BackgroundService."),
        ("Message Queues with RabbitMQ", "Decouple your services by publishing and consuming messages asynchronously through a broker."),
        ("Event-Driven Architecture in .NET", "Design loosely coupled systems that react to domain events instead of calling each other directly."),
        ("Building gRPC Services in .NET", "Create fast, contract-first APIs using Protocol Buffers and strongly typed service definitions."),
        ("Consuming REST APIs with Refit", "Turn an HTTP API into a clean C# interface and let Refit generate the implementation for you."),
        ("HttpClientFactory Best Practices", "Avoid socket exhaustion and configure resilient HTTP clients with named and typed clients."),
        ("Resilience with Polly", "Add retries, circuit breakers, and timeouts to your outbound calls to survive transient failures."),
        ("Delegating Handlers Explained", "Intercept outgoing HTTP requests to add logging, authentication headers, or correlation IDs."),
        ("Structured Logging with Serilog", "Write queryable logs with properties instead of plain strings, and ship them to a central sink."),
        ("OpenTelemetry in ASP.NET Core", "Add distributed tracing and metrics to understand how requests flow through your microservices."),
        ("Health Checks for Production APIs", "Expose readiness and liveness endpoints so orchestrators know when your app is healthy."),
        ("Feature Flags in .NET", "Roll out features gradually and toggle them at runtime without redeploying your application."),
        ("Configuration and Options Pattern", "Bind appsettings.json sections to strongly typed classes and validate them at startup."),
        ("Managing Secrets with User Secrets", "Keep API keys and connection strings out of source control during local development."),
        ("Async and Await Fundamentals", "Understand how the async state machine works and avoid blocking calls that cause deadlocks."),
        ("Cancellation Tokens Done Right", "Propagate cancellation through your call stack so long-running operations stop cleanly."),
        ("Channels for Producer-Consumer Workloads", "Use System.Threading.Channels to build efficient in-memory pipelines between tasks."),
        ("Span and Memory for Zero-Allocation Code", "Slice arrays and buffers without allocating, and write high-performance parsing code."),
        ("Source Generators in C#", "Generate boilerplate code at compile time to improve performance and reduce reflection."),
        ("Records and Pattern Matching", "Write concise immutable types and expressive branching logic with modern C# features."),
        ("Nullable Reference Types", "Let the compiler catch null bugs before they reach production by enabling nullable annotations."),
        ("LINQ Performance Tips", "Avoid multiple enumeration, understand deferred execution, and pick the right operators."),
        ("Unit Testing with xUnit", "Write fast, isolated tests and learn the arrange-act-assert pattern for clean test methods."),
        ("Mocking Dependencies with Moq", "Replace real services with test doubles to test your code in isolation."),
        ("Integration Testing with WebApplicationFactory", "Spin up your whole API in memory and test it end to end against a real test database."),
        ("Test Data with Bogus", "Generate realistic fake data for your tests and seed scripts using the Bogus library."),
        ("Clean Architecture in .NET", "Organize your solution into layers so business rules stay independent of frameworks and databases."),
        ("CQRS with MediatR", "Separate reads from writes and keep your controllers thin by sending commands and queries."),
        ("The Repository Pattern Revisited", "Understand when a repository adds value over using DbContext directly, and when it just adds noise."),
        ("Domain-Driven Design Basics", "Model your business domain with entities, value objects, and aggregates that enforce invariants."),
        ("Vertical Slice Architecture", "Group code by feature instead of by technical layer to reduce coupling and ease navigation."),
        ("Mapping with Mapster", "Convert between entities and DTOs quickly with a fast, convention-based object mapper."),
        ("Pagination for Large Result Sets", "Return data in pages with cursor or offset strategies to keep responses fast and small."),
        ("File Uploads in ASP.NET Core", "Accept and stream large files safely, validate content types, and store them outside the web root."),
        ("Sending Email with MailKit", "Send reliable transactional emails over SMTP with attachments and HTML templates."),
        ("Generating PDFs in .NET", "Produce invoices and reports as PDF documents from your server-side code."),
        ("Working with Excel using ClosedXML", "Read and write spreadsheets without installing Office on the server."),
        ("Scheduling Tasks with Quartz.NET", "Run cron-style scheduled jobs with persistence and clustering support."),
        ("Real-Time Updates with SignalR", "Push live data to browsers and apps over WebSockets with automatic fallback transports."),
        ("Server-Sent Events in ASP.NET Core", "Stream updates to clients over a simple one-way HTTP connection."),
        ("WebSockets from Scratch", "Understand the low-level protocol behind real-time features before reaching for a library."),
        ("Output Caching in .NET 8", "Cache entire responses at the server to dramatically cut down on repeated work."),
        ("Response Compression", "Shrink payloads with gzip and brotli to make your APIs faster over the wire."),
        ("Building a NuGet Package", "Package your reusable library, version it with SemVer, and publish it to nuget.org."),
        ("Multi-Targeting Libraries", "Ship one package that supports several .NET versions from a single project file."),
        ("Analyzers and Code Fixes", "Enforce coding standards automatically with Roslyn analyzers in your build."),
        ("Globalization and Localization", "Translate your app and format dates and numbers correctly for every culture."),
        ("Time Zones and DateTimeOffset", "Store and convert times correctly so your app behaves the same everywhere."),
        ("Working with JSON using System.Text.Json", "Serialize and deserialize efficiently and customize naming, converters, and options."),
        ("Model Binding and Custom Binders", "Control how request data maps onto your action parameters."),
        ("Content Negotiation in Web APIs", "Return JSON, XML, or custom formats based on what the client asks for."),
        ("Versioning Your Web API", "Evolve your API without breaking existing clients using URL, header, or query versioning."),
        ("OpenAPI and Swagger Documentation", "Auto-generate interactive docs and client SDKs from your endpoints."),
        ("Securing Secrets with Azure Key Vault", "Centralize secrets and certificates and access them safely from your app."),
        ("Deploying .NET Apps with Docker", "Containerize your app with a small multi-stage Dockerfile for reproducible deploys."),
        ("Kubernetes for .NET Developers", "Run, scale, and self-heal your containers with deployments, services, and probes."),
        ("CI/CD with GitHub Actions", "Build, test, and deploy your .NET app automatically on every push."),
        ("Blue-Green Deployments", "Release new versions with zero downtime by switching traffic between two environments."),
        ("Profiling Memory Leaks", "Find and fix leaks using dotnet-counters, dump analysis, and the GC heap."),
        ("Reducing GC Pressure", "Cut allocations and avoid large object heap fragmentation for steadier performance."),
        ("Benchmarking with BenchmarkDotNet", "Measure your code accurately and avoid common micro-benchmarking mistakes."),
        ("ArrayPool and Object Pooling", "Reuse buffers and objects to reduce allocations in hot paths."),
        ("Parallel Programming with TPL", "Use Parallel.For and PLINQ to spread CPU-bound work across cores safely."),
        ("Thread Safety and Locking", "Protect shared state with locks, Interlocked, and concurrent collections."),
        ("Securing Cookies and Sessions", "Configure SameSite, Secure, and HttpOnly flags to defend against common attacks."),
        ("Preventing SQL Injection", "Always parameterize queries and understand why string concatenation is dangerous."),
        ("Cross-Site Scripting (XSS) Defenses", "Encode output and apply a content security policy to keep injected scripts from running."),
        ("CSRF Protection in ASP.NET Core", "Use anti-forgery tokens to stop malicious sites from forging requests on behalf of users."),
        ("Security Headers Made Easy", "Add HSTS, X-Content-Type-Options, and CSP headers to harden your responses."),
        ("IP Address Safelisting", "Restrict sensitive endpoints to a known set of trusted IP addresses."),
        ("Hashing Passwords Correctly", "Use a slow, salted algorithm like PBKDF2 or Argon2 instead of plain hashes."),
        ("Data Protection API in ASP.NET Core", "Encrypt and decrypt sensitive values with managed key rotation."),
        ("Full-Text Search in PostgreSQL", "Use tsvector and tsquery to rank and match documents by keywords."),
        ("Vector Search with pgvector", "Store embeddings in PostgreSQL and find the nearest neighbors by cosine distance."),
        ("Retrieval-Augmented Generation Explained", "Combine your own data with an LLM so answers are grounded in real, up-to-date content."),
        ("Generating Text Embeddings", "Turn sentences into numeric vectors that capture meaning for semantic search."),
        ("Streaming LLM Responses in .NET", "Render tokens to the user as they arrive instead of waiting for the full response."),
        ("Chunking Documents for RAG", "Split long content into overlapping passages so retrieval stays accurate."),
        ("Hybrid Search: Keywords plus Vectors", "Blend full-text and vector search to get both precision and semantic recall."),
        ("Indexing Vectors with HNSW", "Speed up nearest-neighbor search with approximate indexes and tune recall versus latency."),
        ("Prompt Engineering for Developers", "Write clear instructions and provide context so the model returns reliable output."),
        ("Function Calling with LLMs", "Let the model invoke your APIs by describing tools it is allowed to call."),
        ("Evaluating RAG Quality", "Measure retrieval relevance and answer faithfulness to catch regressions."),
        ("Building a Documentation Assistant", "Combine embeddings, vector search, and an LLM to answer questions over your docs."),
    ];
}
