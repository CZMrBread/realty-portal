using Server.Features.RealtyAgency.CreateRealtyAgency;
using Server.Features.RealtyAgency.DeleteRealtyAgency;
using Server.Features.RealtyAgency.GetRealtyAgencies;
using Server.Features.RealtyAgency.GetRealtyAgency;
using Server.Features.RealtyAgency.UpdateRealtyAgency;

namespace Server.Features.RealtyAgency;

/// <summary>Route group of the RealtyAgency feature.</summary>
public static class RealtyAgencyEndpoints
{
    /// <summary>Route prefix of the feature.</summary>
    public const string Prefix = "/realty-agency";

    /// <summary>OpenAPI tag of the feature's routes.</summary>
    public const string Tag = "RealtyAgency";

    /// <summary>Registers the agency routes under <see cref="Prefix"/>.</summary>
    public static void MapRealtyAgencyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(Prefix).WithTags(Tag);

        group.MapGetRealtyAgency();
        group.MapGetRealtyAgencies();
        group.MapCreateRealtyAgency();
        group.MapUpdateRealtyAgency();
        group.MapDeleteRealtyAgency();
    }
}
