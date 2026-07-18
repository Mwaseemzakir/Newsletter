using EfCoreInterceptors.Application.Mappers;
using EfCoreInterceptors.Application.Models;
using EfCoreInterceptors.Domain.Entities;
using EfCoreInterceptors.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCoreInterceptors.Controllers;

// ⚠️ Demo only: this controller contains the business logic inline to keep the sample
// in one place and easy to follow. In a real application a controller should only receive
// the request, delegate the work to a service/handler, and return the response — business
// logic does not belong in controllers.
[ApiController]
[Route("api/blogs")]
public sealed class BlogsController(AppDbContext context) : ControllerBase
{
    /// <summary>
    /// CREATE. The handler only sets the title. The AuditInterceptor fills CreatedAt/CreatedBy,
    /// and the OutboxInterceptor writes a BlogCreatedEvent row — none of that is wired here.
    /// Tip: send an <c>X-User</c> header to see it captured in the audit columns.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<BlogResponse>> Create([FromBody] CreateBlogRequest request, CancellationToken ct)
    {
        if(string.IsNullOrWhiteSpace(request.Title))
            return BadRequest("Title is required.");

        var blog = Blog.Create(request.Title);

        context.Blogs.Add(blog);

        await context.SaveChangesAsync(ct);

        return Ok(blog.ToResponse());
    }

    /// <summary>
    /// DELETE. The handler raises the domain event and calls Remove — that's it.
    /// SoftDeleteInterceptor turns the delete into IsDeleted=true, AuditInterceptor stamps
    /// UpdatedBy, and OutboxInterceptor writes a BlogDeletedEvent. The row is never removed.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var blog = await context.Blogs.FirstOrDefaultAsync(b => b.Id == id, ct);

        if(blog is null)
            return NotFound();

        blog.Delete();              // raises BlogDeletedEvent -> outbox
        context.Blogs.Remove(blog); // marked Deleted -> SoftDeleteInterceptor flips it
        await context.SaveChangesAsync(ct);

        return NoContent();
    }

    /// <summary>All blogs the normal way — soft-deleted rows are hidden by the query filter.</summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BlogResponse>>> GetAll(CancellationToken ct)
    {
        var blogs = await context.Blogs
            .OrderBy(b => b.CreatedAt)
            .ToListAsync(ct);

        return Ok(blogs.Select(b => b.ToResponse()).ToList());
    }

    /// <summary>
    /// Same query but with <c>IgnoreQueryFilters()</c>, proving the soft-deleted rows
    /// are still physically in the table (IsDeleted=true) and were never removed.
    /// </summary>
    [HttpGet("with-deleted")]
    public async Task<ActionResult<IReadOnlyList<BlogResponse>>> GetAllWithDeleted(CancellationToken ct)
    {
        var blogs = await context.Blogs
            .IgnoreQueryFilters()
            .OrderBy(b => b.CreatedAt)
            .ToListAsync(ct);

        return Ok(blogs.Select(b => b.ToResponse()).ToList());
    }

    /// <summary>The outbox rows the OutboxInterceptor wrote — one per domain event.</summary>
    [HttpGet("/api/outbox")]
    public async Task<ActionResult<IReadOnlyList<OutboxMessageResponse>>> GetOutbox(CancellationToken ct)
    {
        var messages = await context.OutboxMessages
            .OrderBy(o => o.CreatedAt)
            .ToListAsync(ct);

        return Ok(messages.Select(m => m.ToResponse()).ToList());
    }
}
