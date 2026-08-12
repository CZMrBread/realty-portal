namespace Shared.User.Login;

public sealed record LoginUserResponse
{
    public Guid Id { get; set; }
    public string? UserName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public IList<string> Roles { get; set; } = new List<string>();
    
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset AccessTokenExpiration { get; set; }
    public DateTimeOffset RefreshTokenExpiration { get; set; }
}