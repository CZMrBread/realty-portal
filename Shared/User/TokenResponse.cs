using System.ComponentModel.DataAnnotations;

namespace Shared.User;

/// <summary>Pair of tokens handed out after a successful sign-in or refresh.</summary>
public sealed record TokenResponse
{
    /// <summary>Short-lived token sent with every authenticated request.</summary>
    public string AccessToken { get; set; } = string.Empty;
    /// <summary>Long-lived token exchanged for a new pair once the access token expires.</summary>
    public string RefreshToken { get; set; } = string.Empty;
    /// <summary>Number of seconds the tokens stay valid for.</summary>
    public int ExpiresInSeconds { get; set; }
}