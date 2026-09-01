using Server.Features.RealtyAgency.Entity;
using Shared.RealtyAgency.GetRealtyAgency;
using Shared.Shared;

namespace Server.Features.RealtyAgency.GetRealtyAgencies;

/// <summary>Returns one page of agencies.</summary>
public static class GetRealtyAgencies
{
    /// <summary>Page size when none is given.</summary>
    public const int DefaultPageSize = 20;

    /// <summary>Largest page size allowed.</summary>
    public const int MaxPageSize = 100;

    /// <summary>Registers the list route.</summary>
    public static void MapGetRealtyAgencies(this IEndpointRouteBuilder group)
    {
        group.MapGet("", GetRealtyAgenciesAsync)
            .WithName(nameof(GetRealtyAgenciesAsync));
    }

    /// <summary>Reads one page of agencies, narrowed by <paramref name="name"/> when given; public.</summary>
    /// <param name="name">Name fragment to match; null matches all.</param>
    /// <param name="page">One-based page number.</param>
    /// <param name="pageSize">Page size, at most <see cref="MaxPageSize"/>.</param>
    internal static async Task<IResult> GetRealtyAgenciesAsync(
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
        var items = agencies.Items.Select(a => a.ToGetResponse()).ToList();
        return TypedResults.Ok(new PagedResult<GetRealtyAgencyResponse>(items, agencies.Page, agencies.PageSize,
            agencies.TotalCount));
    }
}
