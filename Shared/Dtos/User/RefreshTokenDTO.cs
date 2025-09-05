using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.User;

public sealed record RefreshTokenDTO
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}