using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Shared.Ruian;
using Shared.Ruian.GetAddressPoints;

namespace Client.Features.Ruian;

/// <summary>Talks to the /ruian read endpoints; a failed call yields an empty list.</summary>
public sealed class RuianApiClient(HttpClient httpClient)
{
    /// <summary>All regions.</summary>
    public async Task<List<RuianPlaceDto>> GetRegionsAsync(CancellationToken cancellationToken = default)
        => await ReadListAsync<RuianPlaceDto>("ruian/regions", cancellationToken);

    /// <summary>Districts of the region.</summary>
    public async Task<List<RuianPlaceDto>> GetDistrictsAsync(int regionCode,
        CancellationToken cancellationToken = default)
        => await ReadListAsync<RuianPlaceDto>($"ruian/regions/{regionCode}/districts", cancellationToken);

    /// <summary>Municipalities of the district.</summary>
    public async Task<List<RuianPlaceDto>> GetMunicipalitiesAsync(int districtCode,
        CancellationToken cancellationToken = default)
        => await ReadListAsync<RuianPlaceDto>($"ruian/districts/{districtCode}/municipalities", cancellationToken);

    /// <summary>Parts of the municipality.</summary>
    public async Task<List<RuianPlaceDto>> GetMunicipalityPartsAsync(int municipalityCode,
        CancellationToken cancellationToken = default)
        => await ReadListAsync<RuianPlaceDto>($"ruian/municipalities/{municipalityCode}/parts", cancellationToken);

    /// <summary>Streets of the municipality; empty where it has none.</summary>
    public async Task<List<RuianPlaceDto>> GetStreetsAsync(int municipalityCode,
        CancellationToken cancellationToken = default)
        => await ReadListAsync<RuianPlaceDto>($"ruian/municipalities/{municipalityCode}/streets", cancellationToken);

    /// <summary>Address points of the street, or the street-less ones of the part.</summary>
    public async Task<List<RuianAddressPointDto>> GetAddressPointsAsync(int? streetCode, int? partCode,
        CancellationToken cancellationToken = default)
    {
        var uri = streetCode is { } street
            ? QueryHelpers.AddQueryString("ruian/address-points", "streetCode", street.ToString())
            : QueryHelpers.AddQueryString("ruian/address-points", "partCode", partCode!.Value.ToString());
        return await ReadListAsync<RuianAddressPointDto>(uri, cancellationToken);
    }

    private async Task<List<T>> ReadListAsync<T>(string uri, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync(uri, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return [];
        }

        return await response.Content.ReadFromJsonAsync<List<T>>(cancellationToken) ?? [];
    }
}
