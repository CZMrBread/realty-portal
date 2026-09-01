using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Features.RealtyAgent;
using Server.Features.User;
using Server.Infrastructure.Database;
using Shared.RealtyAgent;
using Shared.User;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Server.Infrastructure.Authentication;

/// <summary>Issues and revokes access tokens (signed JWTs) and single-use refresh tokens stored as hashes.</summary>
public sealed class AccessTokenService
{
    private const int RefreshTokenSize = 64;
    private static readonly TimeSpan AccessTokenLifetime = TimeSpan.FromMinutes(15);
    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(30);

    private readonly IConfiguration config;
    private readonly AppDbContext dbContext;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RealtyAgentService realtyAgentService;

    /// <summary>Creates the service.</summary>
    /// <exception cref="ArgumentNullException">One of the dependencies is null.</exception>
    public AccessTokenService(IConfiguration configuration, AppDbContext context,
        UserManager<ApplicationUser> userManager, RealtyAgentService realtyAgentService)
    {
        config = configuration ?? throw new ArgumentNullException(nameof(configuration));
        dbContext = context ?? throw new ArgumentNullException(nameof(context));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.realtyAgentService = realtyAgentService ?? throw new ArgumentNullException(nameof(realtyAgentService));
    }

    /// <summary>Issues the first token pair for an authenticated user.</summary>
    public async Task<TokenResponse?> CreateAuthenticationAsync(ApplicationUser user)
    {
        var access = await CreateAccessTokenAsync(user);
        var refresh = CreateRefreshToken();

        dbContext.RefreshTokens.Add(new RefreshTokenEntity()
        {
            UserId = user.Id,
            TokenHash = Hash(refresh),
            ExpiresAt = DateTime.UtcNow.Add(RefreshTokenLifetime)
        });
        await dbContext.SaveChangesAsync();
        return new TokenResponse
        {
            AccessToken = access,
            RefreshToken = refresh,
            ExpiresInSeconds = (int)AccessTokenLifetime.TotalSeconds
        };
    }

    /// <summary>
    /// Exchanges a refresh token for a new pair, or returns null when it is unknown or expired.
    /// A revoked token counts as a replay and revokes every token of that user.
    /// </summary>
    public async Task<TokenResponse?> RefreshAsync(string refreshToken)
    {
        var hash = Hash(refreshToken);
        var token = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == hash);

        if (token is null)
        {
            return null;
        }
        if (token.RevokedAt is not null)
        {
            await RevokeAllForUserAsync(token.UserId);
            return null;
        }

        if (token.IsExpired)
        {
            return null;
        }

        var user = await userManager.FindByIdAsync(token.UserId.ToString());
        if (user is null)
        {
            return null;
        }

        var newRefresh = CreateRefreshToken();
        token.RevokedAt = DateTime.UtcNow;
        token.ReplacedByHash = Hash(newRefresh);

        dbContext.RefreshTokens.Add(new RefreshTokenEntity
        {
            UserId = user.Id,
            TokenHash = token.ReplacedByHash,
            ExpiresAt = DateTime.UtcNow.Add(RefreshTokenLifetime)
        });
        await dbContext.SaveChangesAsync();

        return new TokenResponse()
        {
            AccessToken = await CreateAccessTokenAsync(user),
            RefreshToken = newRefresh,
            ExpiresInSeconds = (int)AccessTokenLifetime.TotalSeconds
        };
    }

    /// <summary>Revokes a single refresh token.</summary>
    public async Task RevokeAsync(string refreshToken)
    {
        var hash = Hash(refreshToken);
        await dbContext.RefreshTokens
            .Where(t => t.TokenHash == hash && t.RevokedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.RevokedAt, DateTime.UtcNow));
    }

    /// <summary>Revokes every refresh token of a user, ending all their sessions.</summary>
    public async Task RevokeAllForUserAsync(Guid userId)
    {
        await dbContext.RefreshTokens
            .Where(t => t.UserId == userId && t.RevokedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.RevokedAt, DateTime.UtcNow));
    }

    /// <summary>Builds and signs the access token.</summary>
    private async Task<string> CreateAccessTokenAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, user.UserName!),
        };

        foreach (var role in await userManager.GetRolesAsync(user))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // carried so the SRealty endpoints can authorize straight from the token
        var agent = await realtyAgentService.FindAgentByUserIdAsync(user.Id, CancellationToken.None);
        if (agent is not null)
        {
            claims.Add(new Claim(AgentClaimTypes.AgentRole, agent.AgentRole.ToString()));

            if (agent.RealtyAgencyId is not null)
            {
                claims.Add(new Claim(AgentClaimTypes.RealtyAgencyId, agent.RealtyAgencyId.Value.ToString()));
            }

            if (agent.RealtyAgentRkId is not null)
            {
                claims.Add(new Claim(AgentClaimTypes.RealtyAgentRkId, agent.RealtyAgentRkId));
            }
        }

        var credentials = new SigningCredentials(
            JwtSigningKey.Create(config),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.Add(AccessTokenLifetime),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>Generates a cryptographically random refresh token.</summary>
    private static string CreateRefreshToken() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(RefreshTokenSize));

    /// <summary>SHA-256 hash of a token, the form it is stored in.</summary>
    private static string Hash(string token) =>
        Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}