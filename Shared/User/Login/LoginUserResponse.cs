namespace Shared.User.Login;

/// <summary>Account details and tokens returned after a successful sign-in.</summary>
public sealed record LoginUserResponse
{
    public Guid Id { get; set; }
    public string? UserName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public IList<string> Roles { get; set; } = new List<string>();
    
    public TokenResponse Token { get; set; }
}