using System.Net.Http.Json;
using Client.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using Shared.Shared;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.ListAdverts;

namespace Client.Features.SRealty;

/// <summary>Talks to the /srealty/advert endpoints.</summary>
public sealed class AdvertApiClient(HttpClient httpClient)
{
    /// <summary>Reads one page of the public listing narrowed by <paramref name="filter"/>, in the order <paramref name="sort"/> asks for.</summary>
    public async Task<(PagedResult<SrealityAdvertDto>? Response, string? Error)> GetFilteredAdvertsAsync(
        AdvertFilter filter, AdvertSortEnum sort, int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var uri = new Uri(httpClient.BaseAddress!, "srealty/advert");
        var queryParams = new Dictionary<string, string>
        {
            { "sort", sort.ToString() },
            { "page", page.ToString() },
            { "pageSize", pageSize.ToString() }
        };
        var queryString = QueryHelpers.AddQueryString(uri.ToString(), queryParams!);
        var response = await httpClient.GetFromJsonAsync<PagedResult<SrealityAdvertDto>>(queryString, cancellationToken);
        return response is not null
            ? (response, null)
            : (null, "Failed to fetch adverts.");
    }
}
