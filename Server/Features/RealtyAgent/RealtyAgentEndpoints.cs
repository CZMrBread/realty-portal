using Server.Features.RealtyAgent.BecomeAgent;
using Server.Features.RealtyAgent.CreateRealtyAgent;
using Server.Features.RealtyAgent.DeleteRealtyAgent;
using Server.Features.RealtyAgent.GetRealtyAgent;
using Server.Features.RealtyAgent.GetRealtyAgents;
using Server.Features.RealtyAgent.UpdateRealtyAgent;

namespace Server.Features.RealtyAgent;

/// <summary>Collects every route of the RealtyAgent feature under one group.</summary>
public static class RealtyAgentEndpoints
{
    /// <summary>Path every route of the feature hangs under.</summary>
    public const string Prefix = "/realty-agent";

    /// <summary>OpenAPI tag the routes are listed under, so that they show up as one category.</summary>
    public const string Tag = "RealtyAgent";

    /// <summary>Registers the agent routes under <see cref="Prefix"/>.</summary>
    public static void MapRealtyAgentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(Prefix).WithTags(Tag);

        group.MapBecomeAgent();
        group.MapGetRealtyAgent();
        group.MapGetRealtyAgents();
        group.MapCreateRealtyAgent();
        group.MapUpdateRealtyAgent();
        group.MapDeleteRealtyAgent();
    }
}
