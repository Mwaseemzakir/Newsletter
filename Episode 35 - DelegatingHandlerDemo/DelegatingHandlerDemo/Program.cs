using DelegatingHandlerDemo.DelegateHandlers;
using DelegatingHandlerDemo.Services;
using Refit;

namespace DelegatingHandlerDemo;
public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddControllers();

		builder.Services.AddEndpointsApiExplorer();

		builder.Services.AddSwaggerGen();

		builder.Services.AddScoped<LoggingHandler>();
		//TODO : Best way is to read the base address from appsettings.json
		builder.Services.AddRefitClient<IDogsAPI>()
			.ConfigureHttpClient(client =>
				client.BaseAddress = new Uri("http://dog-api.kinduff.com/api")
			).AddHttpMessageHandler<LoggingHandler>();

		builder.Services.AddScoped<IDogsService, DogsService>();

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