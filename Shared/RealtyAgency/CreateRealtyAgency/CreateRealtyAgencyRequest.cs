using System.ComponentModel.DataAnnotations;

namespace Shared.RealtyAgency.CreateRealtyAgency;

/// <summary>A new agency; the portal assigns the identifier.</summary>
public sealed record CreateRealtyAgencyRequest
{
    [Required]
    [MaxLength(200)]
    public string? Name { get; set; }

    /// <summary>Company registration number of the agency.</summary>
    [Required]
    [MaxLength(32)]
    public string? RegistrationNumber { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string? Email { get; set; }

    [Phone]
    [MaxLength(32)]
    public string? PhoneNumber { get; set; }

    /// <summary>Street and house number of the office.</summary>
    [MaxLength(200)]
    public string? Street { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(16)]
    public string? PostalCode { get; set; }
}
