namespace Server.Infrastructure.Database;

/// <summary>An entity whose identifier and timestamps <see cref="AppDbContext"/> maintains on save.</summary>
public interface ITimeStampedEntity
{
    public Guid Id { get; set; }
    /// <summary>When the row was inserted.</summary>
    public DateTimeOffset CreatedAt { get; set; }
    /// <summary>When the row was last written.</summary>
    public DateTimeOffset UpdatedAt { get; set; }
}