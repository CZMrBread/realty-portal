namespace Server.Infrastructure.Database;

/// <summary>
/// An entity whose identifier and timestamps the context maintains. Implementing it is what makes
/// <see cref="AppDbContext"/> stamp the entity on save and check that its identifier is a version 7 GUID.
/// </summary>
public interface ITimeStampedEntity
{
    public Guid Id { get; set; }
    /// <summary>When the row was first written. Set once, on insert.</summary>
    public DateTimeOffset CreatedAt { get; set; }
    /// <summary>When the row was last written.</summary>
    public DateTimeOffset UpdatedAt { get; set; }
}