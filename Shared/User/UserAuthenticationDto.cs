namespace Shared.User;

public sealed record UserAuthenticationDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset AccessTokenExpiration { get; set; }
    public DateTimeOffset RefreshTokenExpiration { get; set; }
    public UserInfoDto User { get; set; } = null!;
}