using InMemoryCache.Abstractions.Cache;
using InMemoryCache.Abstractions.PrimeNumbers;
using InMemoryCache.Services.Cache;
using InMemoryCache.Services.PrimeNumbers;

namespace InMemoryCache
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<IMemoryCacheOptions, MemoryCacheOptions>();
            builder.Services.AddScoped<IPrimeNumbersService, PrimeNumbersService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}