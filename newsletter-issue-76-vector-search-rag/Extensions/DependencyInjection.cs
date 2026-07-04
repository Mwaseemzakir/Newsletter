using Microsoft.EntityFrameworkCore;
using VectorSearchRag.Infrastructure.Data;
using VectorSearchRag.Application.Abstractions;
using VectorSearchRag.Infrastructure.Services;

namespace VectorSearchRag.Extensions;

/// <summary>
/// Service-registration extensions so <c>Program.cs</c> stays a thin, readable
/// composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>Registers the PostgreSQL + pgvector <see cref="AppDbContext"/>.</summary>
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres")
            ?? throw new InvalidOperationException("Connection string 'Postgres' is missing.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql => npgsql.UseVector()));

        return services;
    }

    /// <summary>
    /// Registers the embedding + answer providers: Gemini when a key is configured,
    /// otherwise the offline local fallback so the demo runs without any API key.
    /// </summary>
    public static IServiceCollection AddAiServices(this IServiceCollection services, IConfiguration configuration)
    {
        var geminiApiKey = configuration["Gemini:ApiKey"];
        var hasGemini = !string.IsNullOrWhiteSpace(geminiApiKey);

        if (hasGemini)
        {
            // Named HttpClients so we can inject the API key into the concrete services.
            services.AddHttpClient(nameof(GeminiEmbeddingService));
            services.AddHttpClient(nameof(GeminiAnswerService));

            services.AddScoped<IEmbeddingService>(sp =>
                new GeminiEmbeddingService(
                    sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(GeminiEmbeddingService)),
                    geminiApiKey!));

            services.AddScoped<IAnswerService>(sp =>
                new GeminiAnswerService(
                    sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(GeminiAnswerService)),
                    geminiApiKey!));
        }
        else
        {
            services.AddScoped<IEmbeddingService, LocalEmbeddingService>();
            services.AddScoped<IAnswerService, LocalAnswerService>();
        }

        return services;
    }
}
