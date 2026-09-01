using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Server.Features.RealtyAgency;
using Server.Features.RealtyAgency.Entity;
using Server.Features.SRealty.Advert.Entity;
using Server.Features.User;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgent.Entity;

/// <summary>A real estate agent; shares its primary key with <see cref="ApplicationUser"/>.</summary>
public class RealtyAgentEntity
{
    /// <summary>Primary key; the identifier of the agent's user account.</summary>
    public Guid UserId { get; set; }

    [JsonIgnore]
    public ApplicationUser User { get; set; } = null!;

    /// <summary>Role of the agent within their agency.</summary>
    public AgentRoleEnum AgentRole { get; set; }

    /// <summary>Null until the agent is taken on by an agency.</summary>
    public Guid? RealtyAgencyId { get; set; }

    [JsonIgnore]
    public RealtyAgencyEntity? RealtyAgency { get; set; }

    /// <summary>Key of the agent in the agency's own system; unique only within that agency.</summary>
    [MaxLength(64)]
    public string? RealtyAgentRkId { get; set; }

    /// <summary>Adverts the agent sells.</summary>
    [JsonIgnore]
    public List<SrealityAdvertEntity> SRealtyProperties { get; set; } = [];
    
    /// <summary>Company registration number (IČO) of the agent.</summary>
    [MaxLength(32)]
    public string RegistrationNumber { get; set; } = null!;

    /// <summary>Whether the agent administers the given agency.</summary>
    public bool IsAdminOf(Guid agencyId) => RealtyAgencyId == agencyId && AgentRole == AgentRoleEnum.AgencyAdmin;
}
