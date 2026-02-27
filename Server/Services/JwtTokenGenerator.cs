using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Database;
using Server.Entities.Users;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Server.Services;

public class JwtTokenGenerator
{
    private const int RefreshTokenSize = 64;
    private const int AccessTokenExpiryMinutes = 60;
    private const int RefreshTokenExpiryDays = 7;
    
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public JwtTokenGenerator(IConfiguration configuration, AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
        
        var roles = await _userManager.GetRolesAsync(user);
        var claims = await GetClaimsAsync(user, roles);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(AccessTokenExpiryMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<RefreshTokenEntity> GenerateRefreshTokenAsync(ApplicationUser user, string? ipAddress = null)
    {
        var refreshToken = new RefreshTokenEntity
        {
            Token = GenerateRandomToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(RefreshTokenExpiryDays),
            UserId = user.Id
        };

        await RevokeOldRefreshTokensAsync(user);

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<(string AccessToken, RefreshTokenEntity RefreshToken)> RefreshTokenAsync(string token, string? ipAddress = null)
    {
        var refreshToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token);

        if (refreshToken == null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        var user = refreshToken.User;
        if (user == null)
        {
            throw new SecurityTokenException("User not found");
        }

        var newRefreshToken = await GenerateRefreshTokenAsync(user, ipAddress);
        
        refreshToken.ReplacedByToken = newRefreshToken.Token;

        await _context.SaveChangesAsync();

        var newAccessToken = await GenerateAccessTokenAsync(user);

        return (newAccessToken, newRefreshToken);
    }

    public async Task<bool> RevokeTokenAsync(string token, string? ipAddress = null, string? reason = null)
    {
        var refreshToken = await _context.RefreshTokens.AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == token);

        if (refreshToken == null)
            return false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task RevokeAllUserTokensAsync(Guid userId)
    {
        var tokens = await _context.RefreshTokens.Where(rt => rt.UserId == userId).ToListAsync();

        _context.RemoveRange(tokens);
        await _context.SaveChangesAsync();
    }
    

    private async Task<List<Claim>> GetClaimsAsync(ApplicationUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.CreateVersion7().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        return claims;
    }

    private static string GenerateRandomToken()
    {
        var randomBytes = new byte[RefreshTokenSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private async Task RevokeOldRefreshTokensAsync(ApplicationUser user)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-30);
        var expiredTokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == user.Id && 
                        (rt.ExpiresAt <= DateTime.UtcNow || rt.CreatedAt <= cutoffDate))
            .ToListAsync();

        _context.RefreshTokens.RemoveRange(expiredTokens);
    }
}