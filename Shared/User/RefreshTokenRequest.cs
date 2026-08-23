using System.ComponentModel.DataAnnotations;

namespace Shared.User;


/// <summary>Asks for a new pair of tokens in exchange for a valid refresh token.</summary>
public sealed record RefreshTokenRequest
{
    [Required]
    public required string RefreshToken { get; init; }
}