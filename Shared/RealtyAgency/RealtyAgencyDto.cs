using System.ComponentModel.DataAnnotations;

namespace Shared.RealtyAgency;

/// <summary>An agency as it travels between the portal and its clients.</summary>
public sealed record RealtyAgencyDto
{
    /// <summary>Identifier the portal knows the agency under. Assigned by the portal, so a create ignores it.</summary>
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Name { get; set; }

    /// <summary>Company registration number the agency is entered in the business register under.</summary>
    [Required]
    [MaxLength(32)]
    public string? RegistrationNumber { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string? Email { get; set; }
}
