using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Server.Features.RealtyAgency;
using Server.Features.RealtyAgency.Entity;
using Server.Features.SRealty.Advert.Entity;
using Server.Features.User;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgent.Entity;

/// <summary>
/// A real estate agent. Shares its primary key with <see cref="ApplicationUser"/> and has no Id of its own.
/// </summary>
public class RealtyAgentEntity
{
    /// <summary>Primary key of the agent, which is the identifier of the user account they sign in with.</summary>
    public Guid UserId { get; set; }

    [JsonIgnore]
    public ApplicationUser User { get; set; } = null!;

    /// <summary>What the agent is allowed to do within their agency.</summary>
    public AgentRoleEnum AgentRole { get; set; }

    /// <summary>Null until the agent is taken on by an agency.</summary>
    public Guid? RealtyAgencyId { get; set; }

    [JsonIgnore]
    public RealtyAgencyEntity? RealtyAgency { get; set; }

    /// <summary>Key of the agent in the agency own system. Unique within one agency, not globally.</summary>
    [MaxLength(64)]
    public string? RealtyAgentRkId { get; set; }

    /// <summary>Adverts this agent is named on as the seller.</summary>
    [JsonIgnore]
    public List<SrealityAdvertEntity> SRealtyProperties { get; set; } = [];
}
