namespace Shared.Dtos.User;

public sealed record UserAuthenticationDTO
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset AccessTokenExpiration { get; set; }
    public DateTimeOffset RefreshTokenExpiration { get; set; }
    public UserInfoDTO User { get; set; } = null!;
}