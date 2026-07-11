using OutboxPattern.Extensions;
using OutboxPattern.Infrastructure.Data;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddOutboxProcessing();

// Wolverine scans this assembly for message handlers (e.g. WelcomeEmailHandler) and
// routes outbox events to them — no hand-rolled dispatcher.
builder.Host.UseWolverine();

var app = builder.Build();

// Apply migrations (creates the Users + OutboxMessages tables) on startup.
await DatabaseInitializer.InitializeAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
