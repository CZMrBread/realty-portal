using Server.Features.RealtyAgent.BecomeAgent;

namespace Server.Features.RealtyAgent;

/// <summary>Collects every route of the RealtyAgent feature under one group.</summary>
public static class RealtyAgentEndpoints
{
    /// <summary>Registers the agent routes under /realtyagent.</summary>
    public static void MapRealtyAgentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/realtyagent").WithTags("RealtyAgent");
        group.MapBecomeAgent();
    }
}
