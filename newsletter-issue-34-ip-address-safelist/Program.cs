using IpAddressSafeListAPI.Filters.Action;
using IpAddressSafeListAPI.Middlewares;

namespace IpAddressSafeListAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var safelist = builder.Configuration["SafeIPs"];

			builder.Services.AddScoped(provider =>
			{
				var logger = provider.GetService<ILogger<WhitelistIpMiddleware>>();

				return new WhitelistIpMiddleware(safelist, logger);
			});

			builder.Services.AddScoped(provider =>
			{
				var logger = provider.GetService<ILogger<WhitelistIpActionFilter>>();

				return new WhitelistIpActionFilter(safelist, logger);
			});

			builder.Services.AddControllers();

			builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();

				app.UseSwaggerUI();
			}
			app.UseMiddleware<WhitelistIpMiddleware>();


			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}