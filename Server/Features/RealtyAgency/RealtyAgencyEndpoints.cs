using Server.Features.RealtyAgency.CreateRealtyAgency;
using Server.Features.RealtyAgency.DeleteRealtyAgency;
using Server.Features.RealtyAgency.GetRealtyAgencies;
using Server.Features.RealtyAgency.GetRealtyAgency;
using Server.Features.RealtyAgency.UpdateRealtyAgency;

namespace Server.Features.RealtyAgency;

/// <summary>Collects every route of the RealtyAgency feature under one group.</summary>
public static class RealtyAgencyEndpoints
{
    /// <summary>Path every route of the feature hangs under.</summary>
    public const string Prefix = "/realty-agency";

    /// <summary>OpenAPI tag the routes are listed under, so that they show up as one category.</summary>
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
