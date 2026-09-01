using Shared.RealtyAgent;

namespace Client.Features.User;

/// <summary>Claims read from the access token by <see cref="AccessTokenParser"/>.</summary>
public sealed record AccessTokenClaims
{
    public Guid UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public IReadOnlyList<string> Roles { get; init; } = [];

    /// <summary>Null when the account is not an agent.</summary>
    public AgentRoleEnum? AgentRole { get; init; }

    /// <summary>Null when the agent belongs to no agency or the account is not an agent.</summary>
    public Guid? AgencyId { get; init; }

    /// <summary>Key of the agent in their agency's own system; null while they belong to no agency.</summary>
    public string? AgentRkId { get; init; }

    public DateTimeOffset ExpiresAt { get; init; }

    /// <summary>Whether the token is expired; the last 30 seconds before expiry already count as expired.</summary>
    public bool IsExpired => DateTimeOffset.UtcNow >= ExpiresAt - TimeSpan.FromSeconds(30);

    public bool IsInRole(string role) => Roles.Contains(role, StringComparer.Ordinal);

    /// <summary>Whether the account is an agent, whichever agent role it holds.</summary>
    public bool IsAgent => AgentRole is not null;

    public bool IsAgencyAdmin => AgentRole == AgentRoleEnum.AgencyAdmin;
}
