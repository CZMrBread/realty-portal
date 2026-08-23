using Shared.RealtyAgent;

namespace Client.Features.User;

/// <summary>What the access token says about the signed-in user. Built by <see cref="AccessTokenParser"/>.</summary>
public sealed record AccessTokenClaims
{
    public Guid UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public IReadOnlyList<string> Roles { get; init; } = [];

    /// <summary>Null when the account is not an agent.</summary>
    public AgentRoleEnum? AgentRole { get; init; }

    /// <summary>Null while the agent belongs to no agency, and always null for an account that is not an agent.</summary>
    public Guid? AgencyId { get; init; }

    /// <summary>Key of the agent in their agency own system, null while they belong to no agency.</summary>
    public string? AgentRkId { get; init; }

    public DateTimeOffset ExpiresAt { get; init; }

    /// <summary>
    /// Whether the token is spent. A token within half a minute of running out counts as spent, which matches the
    /// clock skew the server allows and keeps a request from being sent with a token that dies on the way.
    /// </summary>
    public bool IsExpired => DateTimeOffset.UtcNow >= ExpiresAt - TimeSpan.FromSeconds(30);

    public bool IsInRole(string role) => Roles.Contains(role, StringComparer.Ordinal);

    /// <summary>Whether the account is an agent at all, whichever agent role it holds.</summary>
    public bool IsAgent => AgentRole is not null;

    public bool IsAgencyAdmin => AgentRole == AgentRoleEnum.AgencyAdmin;
}
