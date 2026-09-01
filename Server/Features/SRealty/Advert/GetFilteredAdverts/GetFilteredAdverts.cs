using Server.Features.SRealty.Advert.Entity;
using Shared.Shared;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.ListAdverts;

namespace Server.Features.SRealty.Advert.GetFilteredAdverts;

/// <summary>Returns one page of the adverts matching a filter.</summary>
public static class GetFilteredAdverts
{
    /// <summary>Default page size.</summary>
    public const int DefaultPageSize = 20;

    /// <summary>Largest allowed page size.</summary>
    public const int MaxPageSize = 100;

    /// <summary>Registers the public listing route.</summary>
    public static void MapGetFilteredAdverts(this IEndpointRouteBuilder group)
    {
        group.MapGet("", GetFilteredAdvertsAsync)
            .WithName("GetFilteredAdverts");
    }

    /// <summary>
    /// Reads one page of unexpired adverts matching <paramref name="filter"/>, ordered by <paramref name="sort"/>.
    /// Anonymous.
    /// </summary>
    /// <param name="filter">Criteria from the query string; an absent one is no restriction.</param>
    /// <param name="sort">Order of the page.</param>
    /// <param name="page">One-based page number.</param>
    /// <param name="pageSize">Adverts per page, at most <see cref="MaxPageSize"/>.</param>
    private static async Task<IResult> GetFilteredAdvertsAsync(
        [AsParameters] AdvertFilter filter,
        AdvertService advertService,
        CancellationToken cancellationToken,
        AdvertSortEnum sort = AdvertSortEnum.Newest,
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

        if (!Enum.IsDefined(sort))
        {
            errors[nameof(sort)] = ["Unknown sort order."];
        }

        if (errors.Count > 0)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var adverts = await advertService.GetAdvertsAsync(filter, sort, page, pageSize, cancellationToken);
        var items = adverts.Items.Select(a => a.ToDto()).ToList();
        return TypedResults.Ok(new PagedResult<SrealityAdvertDto>(items, adverts.Page, adverts.PageSize,
            adverts.TotalCount));
    }
}
