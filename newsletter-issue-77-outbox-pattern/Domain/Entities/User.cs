namespace OutboxPattern.Domain.Entities;

/// <summary>
/// A registered user. Kept deliberately small — the whole point of this issue is the
/// outbox row that gets written alongside it, not the user model itself.
/// </summary>
public sealed class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    private User() { } // EF

    /// <summary>Factory for a new user. No side effects here — those go to the outbox.</summary>
    public static User Create(string email) => new()
    {
        Id = Guid.NewGuid(),
        Email = email,
        CreatedAt = DateTime.UtcNow
    };
}
