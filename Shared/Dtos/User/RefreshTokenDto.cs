using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.User;

public sealed record RefreshTokenDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}