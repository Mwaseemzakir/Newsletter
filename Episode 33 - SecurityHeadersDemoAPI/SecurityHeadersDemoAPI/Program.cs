using SecurityHeadersDemoAPI.Middlewares;

namespace SecurityHeadersDemoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<SecureHeadersMiddleware>();
            builder.Services.AddAntiforgery(options =>
            {
                //Set options to avoid click jacking attack
                options.SuppressXFrameOptionsHeader = true;
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<SecureHeadersMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}