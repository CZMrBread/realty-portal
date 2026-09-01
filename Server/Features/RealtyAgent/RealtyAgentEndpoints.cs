using Server.Features.RealtyAgent.BecomeAgent;
using Server.Features.RealtyAgent.CreateRealtyAgent;
using Server.Features.RealtyAgent.DeleteRealtyAgent;
using Server.Features.RealtyAgent.GetRealtyAgent;
using Server.Features.RealtyAgent.GetRealtyAgents;
using Server.Features.RealtyAgent.UpdateRealtyAgent;

namespace Server.Features.RealtyAgent;

/// <summary>Route group of the RealtyAgent feature.</summary>
public static class RealtyAgentEndpoints
{
    /// <summary>Route prefix of the feature.</summary>
    public const string Prefix = "/realty-agent";

    /// <summary>OpenAPI tag of the feature's routes.</summary>
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
