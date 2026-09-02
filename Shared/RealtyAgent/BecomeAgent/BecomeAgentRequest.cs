using System.ComponentModel.DataAnnotations;

namespace Shared.RealtyAgent.BecomeAgent;

/// <summary>Data an account submits to become an agent.</summary>
public sealed record BecomeAgentRequest
{
    /// <summary>Full name shown to the public.</summary>
    [Required]
    [MaxLength(200)]
    public string? Name { get; set; }

    /// <summary>Company registration number (IČO) of the agent.</summary>
    [Required]
    [MaxLength(32)]
    public string? RegistrationNumber { get; set; }

    /// <summary>Public contact email; independent of the account email.</summary>
    [EmailAddress]
    [MaxLength(256)]
    public string? Email { get; set; }

    /// <summary>Public contact phone number.</summary>
    [Phone]
    [MaxLength(32)]
    public string? PhoneNumber { get; set; }
}
