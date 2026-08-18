using System.ComponentModel.DataAnnotations;
using Server.Features.User;
using Server.Infrastructure.Database;

namespace Server.Features.RealtyAgency;

public sealed class RealtyAgencyEntity : ITimeStampedEntity
{
    public RealtyAgencyEntity(string Name, string RegistrationNumber, string Email)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        this.Name = Name;
        this.RegistrationNumber = RegistrationNumber;
        this.Email = Email;
    }
    [Key] public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string RegistrationNumber { get; set; }
    public string? TaxNumber { get; set; }
    [Url] public string? WebsiteUrl { get; set; }

    [EmailAddress] [Required] public string Email { get; set; }

    public ICollection<ApplicationUser> RealtyAgent { get; set; } = new List<ApplicationUser>();
}