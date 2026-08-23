using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Shared.RealtyAgent;

namespace Client.Tests.TestDoubles;

/// <summary>
/// Builds access tokens shaped like the ones the server issues. The signature is nonsense on purpose: the
/// client reads the payload and never checks the signature, so a test that needed a real one would be
/// testing something the client does not do.
/// </summary>
public static class TestTokens
{
    public static string Create(
        Guid? userId = null,
        string userName = "tester",
        string[]? roles = null,
        AgentRoleEnum? agentRole = null,
        Guid? agencyId = null,
        string? agentRkId = null,
        TimeSpan? expiresIn = null)
    {
        var payload = new JsonObject
        {
            ["sub"] = (userId ?? Guid.CreateVersion7()).ToString(),
            ["jti"] = Guid.NewGuid().ToString(),
            [ClaimTypes.Name] = userName,
            ["exp"] = DateTimeOffset.UtcNow.Add(expiresIn ?? TimeSpan.FromMinutes(15)).ToUnixTimeSeconds()
        };

        if (roles is { Length: 1 })
        {
            // one role is written as a bare string, several as an array, exactly as the server writes them
            payload[ClaimTypes.Role] = roles[0];
        }
        else if (roles is { Length: > 1 })
        {
            payload[ClaimTypes.Role] = new JsonArray(roles.Select(r => (JsonNode)r!).ToArray());
        }

        if (agentRole is not null)
        {
            payload[AgentClaimTypes.AgentRole] = agentRole.ToString();
        }

        if (agencyId is not null)
        {
            payload[AgentClaimTypes.RealtyAgencyId] = agencyId.ToString();
        }

        if (agentRkId is not null)
        {
            payload[AgentClaimTypes.RealtyAgentRkId] = agentRkId;
        }

        return $"{Encode("""{"alg":"HS256","typ":"JWT"}""")}.{Encode(payload.ToJsonString())}.not-a-real-signature";
    }

    /// <summary>Body of a successful sign-in, so a stubbed server can answer the way the real one does.</summary>
    public static string LoginResponseJson(string accessToken, string refreshToken = "refresh-token")
        => JsonSerializer.Serialize(new
        {
            id = Guid.CreateVersion7(),
            userName = "tester",
            email = "tester@example.com",
            roles = Array.Empty<string>(),
            token = new { accessToken, refreshToken, expiresInSeconds = 900 }
        });

    /// <summary>Body of a successful refresh.</summary>
    public static string TokenResponseJson(string accessToken, string refreshToken = "refresh-token")
        => JsonSerializer.Serialize(new { accessToken, refreshToken, expiresInSeconds = 900 });

    private static string Encode(string json)
        => Convert.ToBase64String(Encoding.UTF8.GetBytes(json))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
}
