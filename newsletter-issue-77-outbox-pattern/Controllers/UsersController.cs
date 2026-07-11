using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutboxPattern.Application.Events;
using OutboxPattern.Application.Mappers;
using OutboxPattern.Application.Models;
using OutboxPattern.Domain.Entities;
using OutboxPattern.Infrastructure.Data;
using AppUser = OutboxPattern.Domain.Entities.User;

namespace OutboxPattern.Controllers;

// ⚠️ Demo only: this controller contains the business logic inline to keep the sample
// in one place and easy to follow. In a real application a controller should only receive
// the request, delegate the work to a service/handler, and return the response — business
// logic does not belong in controllers.
[ApiController]
[Route("api/users")]
public sealed class UsersController(AppDbContext context) : ControllerBase
{
    /// <summary>
    /// REGISTER. The welcome email is NOT sent here. Instead we write a single
    /// <see cref="OutboxMessage"/> alongside the user in the SAME SaveChanges call, so the
    /// two either commit together or roll back together. The background job sends the email
    /// a few seconds later. No side effect can be lost if the request dies after the save.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterUserRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest("Email is required.");

        var user = AppUser.Create(request.Email);

        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            // AssemblyQualifiedName (not the short name or FullName): the processor calls
            // Type.GetType(...) to turn this back into a real Type before deserializing, and
            // that only resolves reliably when the string carries the full assembly info.
            Type = typeof(WelcomeEmailRequested).AssemblyQualifiedName!,
            Payload = JsonSerializer.Serialize(new WelcomeEmailRequested(user.Email)),
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(user);
        context.OutboxMessages.Add(outboxMessage);

        // One transaction: the user and its outbox message are committed atomically.
        await context.SaveChangesAsync(ct);

        return Ok(user.ToResponse());
    }

    /// <summary>All registered users, oldest first.</summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetAll(CancellationToken ct)
    {
        var users = await context.Users
            .OrderBy(u => u.CreatedAt)
            .ToListAsync(ct);

        return Ok(users.Select(u => u.ToResponse()).ToList());
    }

    /// <summary>
    /// The outbox rows. Right after a register you'll see <c>ProcessedAt = null</c>; within
    /// ~10 seconds the background job stamps it, proving the email was sent reliably.
    /// </summary>
    [HttpGet("/api/outbox")]
    public async Task<ActionResult<IReadOnlyList<OutboxMessageResponse>>> GetOutbox(CancellationToken ct)
    {
        var messages = await context.OutboxMessages
            .OrderBy(o => o.CreatedAt)
            .ToListAsync(ct);

        return Ok(messages.Select(m => m.ToResponse()).ToList());
    }
}
