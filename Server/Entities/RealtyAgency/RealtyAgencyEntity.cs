using System.ComponentModel.DataAnnotations;

namespace Server.Entities.RealtyAgency;

public class RealtyAgencyEntity : ITimeStampedEntity
{
    public string Name { get; set; } = string.Empty;

    [Key] public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}