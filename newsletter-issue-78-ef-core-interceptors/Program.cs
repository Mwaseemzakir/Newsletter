using EfCoreInterceptors.Infrastructure.Data;
using EfCoreInterceptors.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInterceptors();
builder.Services.AddDatabase(builder.Configuration);

var app = builder.Build();

// Apply migrations (creates the Blogs + OutboxMessages tables) on startup.
await DatabaseInitializer.InitializeAsync(app.Services);

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
