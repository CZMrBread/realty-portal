using Server.Entities.RealtyAgency;

namespace Server.Entities.Users.Realty;

public class RealtyAgencyAdminEntity : RealtyAgentEntity
{
    public RealtyAgencyAdminEntity()
    {
    }

    public RealtyAgencyAdminEntity(RealtyAgencyEntity realtyAgencyEntity) : base(realtyAgencyEntity)
    {
    }
}