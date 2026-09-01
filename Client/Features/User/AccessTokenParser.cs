using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Shared.RealtyAgent;

namespace Client.Features.User;

/// <summary>Reads claims out of an access token without validating its signature.</summary>
public static class AccessTokenParser
{
    /// <summary>Claims of the given token, or null when it is not a readable JWT.</summary>
    public static AccessTokenClaims? Parse(string? accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return null;
        }

        var parts = accessToken.Split('.');
        if (parts.Length != 3)
        {
            return null;
        }

        JsonElement payload;
        try
        {
            payload = JsonSerializer.Deserialize<JsonElement>(DecodeSegment(parts[1]));
        }
        catch (Exception)
        {
            return null;
        }

        if (payload.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        return new AccessTokenClaims
        {
            UserId = ReadGuid(payload, "sub") ?? Guid.Empty,
            UserName = ReadString(payload, ClaimTypes.Name) ?? string.Empty,
            Roles = ReadStrings(payload, ClaimTypes.Role),
            AgentRole = ReadAgentRole(payload),
            AgencyId = ReadGuid(payload, AgentClaimTypes.RealtyAgencyId),
            AgentRkId = ReadString(payload, AgentClaimTypes.RealtyAgentRkId),
            ExpiresAt = ReadExpiry(payload)
        };
    }

    /// <summary>Decodes one base64url segment, restoring the base64 padding.</summary>
    private static byte[] DecodeSegment(string segment)
    {
        var padded = segment.Replace('-', '+').Replace('_', '/');
        padded = padded.PadRight((padded.Length + 3) / 4 * 4, '=');
        return Convert.FromBase64String(padded);
    }

    private static string? ReadString(JsonElement payload, string name)
        => payload.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.String
            ? value.GetString()
            : null;

    private static Guid? ReadGuid(JsonElement payload, string name)
        => Guid.TryParse(ReadString(payload, name), out var parsed) ? parsed : null;

    /// <summary>Reads a claim that may be a single string or an array of strings.</summary>
    private static IReadOnlyList<string> ReadStrings(JsonElement payload, string name)
    {
        if (!payload.TryGetProperty(name, out var value))
        {
            return [];
        }

        return value.ValueKind switch
        {
            JsonValueKind.String => [value.GetString()!],
            JsonValueKind.Array => value.EnumerateArray()
                .Where(item => item.ValueKind == JsonValueKind.String)
                .Select(item => item.GetString()!)
                .ToArray(),
            _ => []
        };
    }

    private static AgentRoleEnum? ReadAgentRole(JsonElement payload)
        => Enum.TryParse<AgentRoleEnum>(ReadString(payload, AgentClaimTypes.AgentRole), out var role) ? role : null;

    private static DateTimeOffset ReadExpiry(JsonElement payload)
        => payload.TryGetProperty("exp", out var value) && value.TryGetInt64(out var seconds)
            ? DateTimeOffset.FromUnixTimeSeconds(seconds)
            : DateTimeOffset.MinValue;
}
