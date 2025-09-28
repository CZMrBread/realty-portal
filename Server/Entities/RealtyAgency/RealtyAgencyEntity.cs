using System.ComponentModel.DataAnnotations;
using Server.Entities.Users;

namespace Server.Entities.RealtyAgency;

public sealed class RealtyAgencyEntity : ITimeStampedEntity
{
    [Key] 
    public Guid Id { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    [Required]
    public string Name { get; set; }

    [Required]
    public string RegistrationNumber { get; set; }
    public string? TaxNumber { get; set; }
    [Url]
    public string? WebsiteUrl { get; set; }

    [EmailAddress]
    [Required]
    public string Email { get; set; }

    public ICollection<ApplicationUser> RealtyAgent { get; set; } = new List<ApplicationUser>();



}