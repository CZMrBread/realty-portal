using System.ComponentModel.DataAnnotations;

namespace Shared.User.Register;

public sealed record RegisterUserRequest
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string UserName { get; set; } = string.Empty;

    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required] [Compare(nameof(Password))] public string ConfirmPassword { get; set; } = string.Empty;

}