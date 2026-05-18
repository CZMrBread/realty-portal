using System.ComponentModel.DataAnnotations;
using Server.RealtyAgency;
using Server.SRealty;
using Server.User;

namespace Server.Entities.Users.Realty;

public class RealtyAgentEntity : ApplicationUser
{
    public RealtyAgentEntity()
    {
    }

    public RealtyAgentEntity(RealtyAgencyEntity realtyAgency)
    {
        RealtyAgency = realtyAgency;
    }

    public RealtyAgencyEntity RealtyAgency { get; set; }
    [MaxLength(64)] public string? RealtyAgentRkId { get; set; } = null;
    public ICollection<SRealtyPropertyEntity> SRealtyProperties { get; set; } = new List<SRealtyPropertyEntity>();
}