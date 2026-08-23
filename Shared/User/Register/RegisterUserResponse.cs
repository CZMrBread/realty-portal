namespace Shared.User.Register;

/// <summary>Account details and tokens returned after a successful registration, so that the new user is signed in straight away.</summary>
public sealed record RegisterUserResponse
{
    public Guid Id { get; set; }
    public string? UserName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public IList<string> Roles { get; set; } = new List<string>();
    
    public TokenResponse Token { get; set; }
}