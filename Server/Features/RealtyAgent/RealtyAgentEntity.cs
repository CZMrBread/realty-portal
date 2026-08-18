using System.ComponentModel.DataAnnotations;
using Server.Features.RealtyAgency;
using Server.Features.SRealty;
using Server.Features.User;

namespace Server.Features.RealtyAgent;

public class RealtyAgentEntity : ApplicationUser
{
    public RealtyAgentEntity(string userName) : base(userName)
    {
    }
    
    

    public RealtyAgencyEntity? RealtyAgency { get; set; }
    [MaxLength(64)] public string? RealtyAgentRkId { get; set; } = null;
    // public ICollection<SRealtyPropertyEntity> SRealtyProperties { get; set; } = new List<SRealtyPropertyEntity>();
}