using System.ComponentModel.DataAnnotations;

namespace Shared.User.UpdateCurrentUser;

/// <summary>Replacement account details of the signed-in user.</summary>
public sealed record UpdateCurrentUserRequest
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string UserName { get; set; } = string.Empty;

    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;
}
