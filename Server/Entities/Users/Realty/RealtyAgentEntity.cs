using Server.Entities.RealtyAgency;

namespace Server.Entities.Users.Realty;

public class RealtyAgentEntity: ApplicationUser
{
    public RealtyAgentEntity(RealtyAgencyEntity realtyAgency)
    {
        RealtyAgency = realtyAgency;
    }

    public Guid RealtyAgencyId { get; set; }
    public RealtyAgencyEntity RealtyAgency { get; set; }
    
}