using System.ComponentModel.DataAnnotations;

namespace Shared.RealtyAgency.UpdateRealtyAgency;

/// <summary>New contents of the agency named by the route.</summary>
public sealed record UpdateRealtyAgencyRequest
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
}
