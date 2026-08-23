using System.ComponentModel.DataAnnotations;

namespace Shared.User.Login;

/// <summary>Credentials sent when signing in.</summary>
public sealed record LoginUserRequest
{
    /// <summary>Email address of the account. The sign-in endpoint also accepts a user name here.</summary>
    [Required] [EmailAddress]
    public string Email { get; set; } = string.Empty;
    

    [Required] public string Password { get; set; } = string.Empty;
}