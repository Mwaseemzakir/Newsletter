using GprcServer.Abstractions;
using GprcServer.Jobs;
using GprcServer.Repositories;
using GprcServer.Services;
namespace GprcServer;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHostedService<PrimeNumbersJob>();

        // Add services to the container
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<IMemoryCacheOptions,MemoryCacheOptions>();
        builder.Services.AddSingleton<IPrimeNumbersRepository, PrimeNumbersRepository>();
        builder.Services.AddGrpc();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<PrimeNumbersService>();
        app.MapGet("/", () => "Communication with gRPC client");
        app.Run();
    }
}