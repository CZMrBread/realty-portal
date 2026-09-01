using System.Globalization;
using System.Net.Http.Json;
using Client.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using Shared.Shared;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.ListAdverts;

namespace Client.Features.SRealty;

/// <summary>Talks to the /srealty/advert endpoints.</summary>
public sealed class AdvertApiClient(HttpClient httpClient)
{
    /// <summary>Path the advert routes live under, relative to the server base address.</summary>
    public const string AdvertPath = "srealty/advert";

    /// <summary>Reads one page of the public listing, filtered and sorted.</summary>
    public async Task<(PagedResult<SrealityAdvertDto>? Response, string? Error)> GetFilteredAdvertsAsync(
        AdvertFilter filter, AdvertSortEnum sort, int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string?>>
        {
            new("sort", sort.ToString()),
            new("page", page.ToString(CultureInfo.InvariantCulture)),
            new("pageSize", pageSize.ToString(CultureInfo.InvariantCulture))
        };
        AppendFilter(query, filter);

        var uri = QueryHelpers.AddQueryString(AdvertPath, query);
        var response = await httpClient.GetAsync(uri, cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<PagedResult<SrealityAdvertDto>>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    /// <summary>Reads the advert with the given portal identifier.</summary>
    public async Task<(SrealityAdvertDto? Response, string? Error)> GetAdvertAsync(Guid advertId,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync($"{AdvertPath}/{advertId}", cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<SrealityAdvertDto>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    /// <summary>Stores a new advert and returns it with its assigned identifier.</summary>
    public async Task<(CreateAdvertResponse? Response, string? Error)> CreateAdvertAsync(SrealityAdvertDto request,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync(AdvertPath, request, cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<CreateAdvertResponse>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    /// <summary>Replaces the contents of an existing advert and returns it as saved.</summary>
    public async Task<(SrealityAdvertDto? Response, string? Error)> UpdateAdvertAsync(Guid advertId,
        SrealityAdvertDto request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PutAsJsonAsync($"{AdvertPath}/{advertId}", request, cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<SrealityAdvertDto>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    /// <summary>Turns the filter into query pairs: nulls are skipped, arrays repeat their key per member.</summary>
    private static void AppendFilter(List<KeyValuePair<string, string?>> query, AdvertFilter filter)
    {
        Append(query, nameof(AdvertFilter.AdvertFunction), filter.AdvertFunction?.ToString());
        Append(query, nameof(AdvertFilter.AdvertType), filter.AdvertType?.ToString());
        foreach (var subtype in filter.AdvertSubtypes ?? [])
        {
            Append(query, nameof(AdvertFilter.AdvertSubtypes), subtype.ToString());
        }

        Append(query, nameof(AdvertFilter.LocalityCity), filter.LocalityCity);
        Append(query, nameof(AdvertFilter.RegionCode), filter.RegionCode?.ToString(CultureInfo.InvariantCulture));
        Append(query, nameof(AdvertFilter.DistrictCode), filter.DistrictCode?.ToString(CultureInfo.InvariantCulture));
        Append(query, nameof(AdvertFilter.MunicipalityCode), filter.MunicipalityCode?.ToString(CultureInfo.InvariantCulture));
        Append(query, nameof(AdvertFilter.PriceFrom), filter.PriceFrom?.ToString(CultureInfo.InvariantCulture));
        Append(query, nameof(AdvertFilter.PriceTo), filter.PriceTo?.ToString(CultureInfo.InvariantCulture));
        Append(query, nameof(AdvertFilter.AreaFrom), filter.AreaFrom?.ToString(CultureInfo.InvariantCulture));
        Append(query, nameof(AdvertFilter.AreaTo), filter.AreaTo?.ToString(CultureInfo.InvariantCulture));
        foreach (var condition in filter.BuildingConditions ?? [])
        {
            Append(query, nameof(AdvertFilter.BuildingConditions), condition.ToString());
        }

        Append(query, nameof(AdvertFilter.Search), filter.Search);
    }

    private static void Append(List<KeyValuePair<string, string?>> query, string name, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            query.Add(new KeyValuePair<string, string?>(name, value));
        }
    }
}
