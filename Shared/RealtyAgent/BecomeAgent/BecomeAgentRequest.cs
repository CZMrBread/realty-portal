using System.ComponentModel.DataAnnotations;

namespace Shared.RealtyAgent.BecomeAgent;

/// <summary>Data an account submits to become an agent.</summary>
public sealed record BecomeAgentRequest
{
    /// <summary>Company registration number (IČO) of the agent.</summary>
    [Required]
    [MaxLength(32)]
    public string? RegistrationNumber { get; set; }
}
