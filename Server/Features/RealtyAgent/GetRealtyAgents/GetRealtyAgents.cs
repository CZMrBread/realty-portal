using Server.Features.RealtyAgent.Entity;
using Shared.RealtyAgent;
using Shared.Shared;

namespace Server.Features.RealtyAgent.GetRealtyAgents;

/// <summary>Returns one page of agents.</summary>
public static class GetRealtyAgents
{
    /// <summary>Page size when none is given.</summary>
    public const int DefaultPageSize = 20;

    /// <summary>Largest page size allowed.</summary>
    public const int MaxPageSize = 100;

    /// <summary>Registers the list route.</summary>
    public static void MapGetRealtyAgents(this IEndpointRouteBuilder group)
    {
        group.MapGet("", GetRealtyAgentsAsync)
            .WithName(nameof(GetRealtyAgentsAsync));
    }

    /// <summary>Reads one page of agents, narrowed by name and agency when given; public.</summary>
    /// <param name="name">Name fragment to match; null matches all.</param>
    /// <param name="agencyId">Agency whose agents are read; null matches all.</param>
    /// <param name="page">One-based page number.</param>
    /// <param name="pageSize">Page size, at most <see cref="MaxPageSize"/>.</param>
    internal static async Task<IResult> GetRealtyAgentsAsync(
        string? name,
        Guid? agencyId,
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

        var agents = await realtyAgentService.SearchAgentsAsync(name, agencyId, page, pageSize, cancellationToken);
        var items = agents.Items.Select(a => a.ToDto()).ToList();
        return TypedResults.Ok(new PagedResult<RealtyAgentDto>(items, agents.Page, agents.PageSize,
            agents.TotalCount));
    }
}
