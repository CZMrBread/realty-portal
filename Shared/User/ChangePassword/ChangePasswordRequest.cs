using System.ComponentModel.DataAnnotations;

namespace Shared.User.ChangePassword;

/// <summary>Password change of the signed-in user.</summary>
public sealed record ChangePasswordRequest
{
    [Required] public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>Repeated new password; must match NewPassword.</summary>
    [Required] [Compare(nameof(NewPassword))] public string ConfirmNewPassword { get; set; } = string.Empty;
}
