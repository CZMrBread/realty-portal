using Server.Entities.Users.Realty;

namespace Server.RealtyAgency;

public class RealtyAgencyAdminEntity : RealtyAgentEntity
{
    public RealtyAgencyAdminEntity()
    {
    }

    public RealtyAgencyAdminEntity(RealtyAgencyEntity realtyAgencyEntity) : base(realtyAgencyEntity)
    {
    }
}