using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Server.Infrastructure.Authentication;

/// <summary>
/// Single source of the symmetric signing key. Issuing and validating used to derive it from the same
/// configuration value by different means, which produced two different keys and rejected every token;
/// both sides now go through here so the two cannot drift apart again.
/// </summary>
public static class JwtSigningKey
{
    public static SymmetricSecurityKey Create(IConfiguration configuration)
        => new(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
}
