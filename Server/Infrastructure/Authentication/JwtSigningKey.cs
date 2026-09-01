using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Server.Infrastructure.Authentication;

/// <summary>Single source of the symmetric signing key, shared by token issuing and validation.</summary>
public static class JwtSigningKey
{
    public static SymmetricSecurityKey Create(IConfiguration configuration)
        => new(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
}
