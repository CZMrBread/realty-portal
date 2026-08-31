using Shared.RealtyAgency;
using Shared.Shared;

namespace Server.Features.RealtyAgency.GetRealtyAgencies;

/// <summary>Returns one page of agencies.</summary>
public static class GetRealtyAgencies
{
    /// <summary>Number of agencies a page holds when the caller does not say.</summary>
    public const int DefaultPageSize = 20;

    /// <summary>Most agencies a single page may hold.</summary>
    public const int MaxPageSize = 100;

    /// <summary>Registers the route the agency list is read through.</summary>
    public static void MapGetRealtyAgencies(this IEndpointRouteBuilder group)
    {
        group.MapGet("", GetRealtyAgenciesAsync)
            .WithName("GetRealtyAgencies");
    }

    /// <summary>
    /// Reads one page of the agencies on the portal, narrowed by <paramref name="name"/> when one is given.
    /// Open to anyone, since an agency is public.
    /// </summary>
    /// <param name="name">Part of the agency name to match, or null to match every agency.</param>
    /// <param name="page">One-based number of the page to read.</param>
    /// <param name="pageSize">Maximum number of agencies the page holds, at most <see cref="MaxPageSize"/>.</param>
    private static async Task<IResult> GetRealtyAgenciesAsync(
        string? name,
        RealtyAgencyService realtyAgencyService,
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

        var agencies = await realtyAgencyService.SearchAgenciesAsync(name, page, pageSize, cancellationToken);
        var items = agencies.Items.Select(a => a.ToDto()).ToList();
        return TypedResults.Ok(new PagedResult<RealtyAgencyDto>(items, agencies.Page, agencies.PageSize,
            agencies.TotalCount));
    }
}
