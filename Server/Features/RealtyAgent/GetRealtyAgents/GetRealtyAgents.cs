using System.Security.Claims;
using Server.Infrastructure.Http;
using Shared.RealtyAgency;
using Shared.RealtyAgent;
using Shared.Shared;

namespace Server.Features.RealtyAgent.GetRealtyAgents;

/// <summary>Returns one page of the agents of an agency.</summary>
public static class GetRealtyAgents
{
    /// <summary>Number of agents a page holds when the caller does not say.</summary>
    public const int DefaultPageSize = 20;

    /// <summary>Most agents a single page may hold.</summary>
    public const int MaxPageSize = 100;

    /// <summary>Registers the route the agent list is read through.</summary>
    public static void MapGetRealtyAgents(this IEndpointRouteBuilder group)
    {
        group.MapGet("", GetRealtyAgentsAsync)
            .WithName("GetRealtyAgents")
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>
    /// Reads one page of the agents of the agency named by <paramref name="agencyId"/>. Only an agent of that
    /// very agency may read it: the list exposes the agency keys, which are agency-internal.
    /// </summary>
    /// <param name="agencyId">Agency whose agents are read.</param>
    /// <param name="page">One-based number of the page to read.</param>
    /// <param name="pageSize">Maximum number of agents the page holds, at most <see cref="MaxPageSize"/>.</param>
    private static async Task<IResult> GetRealtyAgentsAsync(
        Guid agencyId,
        ClaimsPrincipal principal,
        RealtyAgentService realtyAgentService,
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = DefaultPageSize)
    {
        var errors = new Dictionary<string, string[]>();
        if (page < 1)
        {
            errors[nameof(page)] = ["The page number has to be 1 or more."];
        }

        if (pageSize is < 1 or > MaxPageSize)
        {
            errors[nameof(pageSize)] = [$"The page size has to be between 1 and {MaxPageSize}."];
        }

        if (errors.Count > 0)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var caller = await realtyAgentService.FindCallingAgentAsync(principal, cancellationToken);
        if (caller is null)
        {
            return AgentErrors.NotAnAgent.ToResult();
        }

        if (caller.RealtyAgencyId != agencyId)
        {
            return AgencyErrors.NotOwned.ToResult();
        }

        var agents = await realtyAgentService.GetAgencyAgentsPageAsync(agencyId, page, pageSize, cancellationToken);
        var items = agents.Items.Select(a => a.ToDto()).ToList();
        return TypedResults.Ok(new PagedResult<RealtyAgentDto>(items, agents.Page, agents.PageSize,
            agents.TotalCount));
    }
}
