using InMemoryCacheDemo.Abstractions.Cache;
using InMemoryCacheDemo.Abstractions.PrimeNumbers;
using InMemoryCacheDemo.Services.Cache;
using InMemoryCacheDemo.Services.PrimeNumbers;

namespace InMemoryCacheDemo
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