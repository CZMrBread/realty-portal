using System.Text.RegularExpressions;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Server.Features.Ruian;
using Server.Features.Ruian.Entity;
using Server.Features.SRealty.Advert.Entity;
using Server.Infrastructure.Database;
using Server.Infrastructure.Database.Configuration;
using Shared.Shared;
using Shared.Ruian;
using Shared.Shared.Extensions;
using Shared.SRealty.Advert.Enums;
using Shared.SRealty.Advert.ListAdverts;

namespace Server.Features.SRealty.Advert;

/// <summary>Reads and writes adverts; writes evict the <see cref="AdvertOutputCachePolicy"/> entry.</summary>
public sealed partial class AdvertService(AppDbContext appDbContext, IOutputCacheStore outputCache, RuianService ruianService)
{
    // --- Get ---

    /// <summary>Advert with the given identifier, or null when there is none. Tracked.</summary>
    public async Task<SrealityAdvertEntity?> FindAdvertByIdAsync(Guid advertId,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.SrealityAdverts.Include(a => a.Agency)
            .FirstOrDefaultAsync(a => a.Id == advertId, cancellationToken);
    }

    /// <summary>Advert the given agency knows under the given key, or null when there is none. Tracked.</summary>
    public async Task<SrealityAdvertEntity?> FindAdvertByRkIdAsync(Guid realtyAgencyId, string advertRkId,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.SrealityAdverts
            .FirstOrDefaultAsync(a => a.RealtyAgencyId == realtyAgencyId && a.AdvertRkId == advertRkId,
                cancellationToken);
    }

    /// <summary>One page of unexpired adverts matching the filter, in the requested order. Untracked.</summary>
    public async Task<PagedResult<SrealityAdvertEntity>> GetAdvertsAsync(AdvertFilter filter, AdvertSortEnum sort,
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter(appDbContext.SrealityAdverts.AsNoTracking().Include(a => a.Agency), filter);
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await ApplySort(query, sort)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return new PagedResult<SrealityAdvertEntity>(items, page, pageSize, totalCount);
    }

    // --- Create / Update / Delete ---

    /// <summary>Stores a new advert; the locality must have been resolved first.</summary>
    public async Task<SrealityAdvertEntity> CreateAdvertAsync(SrealityAdvertEntity advert,
        CancellationToken cancellationToken = default)
    {
        appDbContext.SrealityAdverts.Add(advert);
        await appDbContext.SaveChangesAsync(cancellationToken);
        return advert;
    }

    /// <summary>Saves the tracked changes and evicts the cached response; the locality must have been resolved first.</summary>
    public async Task<SrealityAdvertEntity> UpdateAdvertAsync(SrealityAdvertEntity advert,
        CancellationToken cancellationToken = default)
    {
        await appDbContext.SaveChangesAsync(cancellationToken);
        await outputCache.EvictByTagAsync(AdvertOutputCachePolicy.Tag(advert.Id), cancellationToken);
        return advert;
    }

    /// <summary>Removes an advert and evicts its cached response.</summary>
    public async Task DeleteAdvertAsync(SrealityAdvertEntity advert, CancellationToken cancellationToken = default)
    {
        appDbContext.SrealityAdverts.Remove(advert);
        await appDbContext.SaveChangesAsync(cancellationToken);
        await outputCache.EvictByTagAsync(AdvertOutputCachePolicy.Tag(advert.Id), cancellationToken);
    }

    /// <summary>Whether the agent is the seller on any advert.</summary>
    public async Task<bool> HasSellerAdvertsAsync(Guid sellerId, CancellationToken cancellationToken = default)
    {
        return await appDbContext.SrealityAdverts.AnyAsync(a => a.SellerId == sellerId, cancellationToken);
    }

    /// <summary>Detaches every advert of an agency: clears the agency link and both agency-scoped keys.</summary>
    public async Task DetachAgencyAdvertsAsync(Guid realtyAgencyId, CancellationToken cancellationToken = default)
    {
        var ids = await appDbContext.SrealityAdverts.Where(a => a.RealtyAgencyId == realtyAgencyId)
            .Select(a => a.Id).ToListAsync(cancellationToken);
        await appDbContext.SrealityAdverts.Where(a => a.RealtyAgencyId == realtyAgencyId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(a => a.RealtyAgencyId, (Guid?)null)
                .SetProperty(a => a.AdvertRkId, (string?)null)
                .SetProperty(a => a.SellerRkId, (string?)null)
                .SetProperty(a => a.UpdatedAt, DateTimeOffset.UtcNow), cancellationToken);
        foreach (var id in ids)
        {
            await outputCache.EvictByTagAsync(AdvertOutputCachePolicy.Tag(id), cancellationToken);
        }
    }

    // --- Listing ---

    /// <summary>The query narrowed by the filter; expired adverts are always left out.</summary>
    private static IQueryable<SrealityAdvertEntity> ApplyFilter(IQueryable<SrealityAdvertEntity> query,
        AdvertFilter filter)
    {
        var now = DateTimeOffset.UtcNow;
        query = query.Where(a => a.ExpiresAt > now);

        if (filter.AdvertFunction is { } advertFunction)
        {
            query = query.Where(a => a.AdvertFunction == advertFunction);
        }

        if (filter.AdvertType is { } advertType)
        {
            query = query.Where(a => a.AdvertType == advertType);
        }

        if (filter.AdvertSubtypes is { Length: > 0 } subtypes)
        {
            query = query.Where(a => subtypes.Contains(a.AdvertSubtype));
        }

        if (!string.IsNullOrWhiteSpace(filter.LocalityCity))
        {
            var city = filter.LocalityCity.Trim().ToLower();
            query = query.Where(a => a.LocalityCity.ToLower().StartsWith(city));
        }

        if (filter.RegionCode is { } regionCode)
        {
            query = query.Where(a => a.LocalityDistrict!.RegionCode == regionCode);
        }

        if (filter.DistrictCode is { } districtCode)
        {
            query = query.Where(a => a.LocalityDistrictCode == districtCode);
        }

        if (filter.MunicipalityCode is { } municipalityCode)
        {
            query = query.Where(a => a.LocalityMunicipalityCode == municipalityCode);
        }

        if (filter.PriceFrom is { } priceFrom)
        {
            query = query.Where(a => a.AdvertPrice >= priceFrom);
        }

        if (filter.PriceTo is { } priceTo)
        {
            query = query.Where(a => a.AdvertPrice <= priceTo);
        }

        // which area counts depends on the type: land has no usable area, only the estate itself
        if (filter.AreaFrom is { } areaFrom)
        {
            query = query.Where(a =>
                (a.AdvertType == AdvertTypeEnum.Land ? a.EstateArea : a.UsableArea) >= areaFrom);
        }

        if (filter.AreaTo is { } areaTo)
        {
            query = query.Where(a =>
                (a.AdvertType == AdvertTypeEnum.Land ? a.EstateArea : a.UsableArea) <= areaTo);
        }

        if (filter.BuildingConditions is { Length: > 0 } conditions)
        {
            query = query.Where(a => a.BuildingCondition != null && conditions.Contains(a.BuildingCondition.Value));
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(a => a.SearchVector!.Matches(
                EF.Functions.PlainToTsQuery(SrealityAdvertConfiguration.TextSearchConfiguration, search)));
        }

        if (filter.RealtyAgencyId is { } realtyAgencyId)
        {
            query = query.Where(a => a.RealtyAgencyId == realtyAgencyId);
        }

        if (filter.SellerId is { } sellerId)
        {
            query = query.Where(a => a.SellerId == sellerId);
        }

        return query;
    }

    /// <summary>The query in the requested order, newest first among equals.</summary>
    private static IOrderedQueryable<SrealityAdvertEntity> ApplySort(IQueryable<SrealityAdvertEntity> query,
        AdvertSortEnum sort)
    {
        var ordered = sort switch
        {
            AdvertSortEnum.RecentlyUpdated => query.OrderByDescending(a => a.UpdatedAt),
            AdvertSortEnum.PriceAscending => query.OrderBy(a => a.AdvertPrice),
            AdvertSortEnum.PriceDescending => query.OrderByDescending(a => a.AdvertPrice),
            AdvertSortEnum.AreaAscending => query.OrderBy(a =>
                a.AdvertType == AdvertTypeEnum.Land ? a.EstateArea : a.UsableArea),
            AdvertSortEnum.AreaDescending => query.OrderByDescending(a =>
                a.AdvertType == AdvertTypeEnum.Land ? a.EstateArea : a.UsableArea),
            _ => query.OrderByDescending(a => a.CreatedAt)
        };
        return ordered.ThenByDescending(a => a.Id);
    }

    // --- Locality ---

    /// <summary>
    /// Checks the advert's locality against the register and fills the resolved codes, names and position.
    /// A RUIAN code is authoritative: it must exist at its level and agree with the town name, and the register
    /// then overwrites the names. Without a code the names are geocoded as far as they match; an unknown town is
    /// accepted unplaced. Returns validation errors keyed by JSON field, empty when the locality is fine.
    /// </summary>
    public async Task<Dictionary<string, string[]>> ResolveLocalityAsync(SrealityAdvertEntity advert,
        CancellationToken cancellationToken = default)
    {
        advert.LocalityMunicipalityCode = null;
        advert.LocalityDistrictCode = null;
        advert.LocalityAddressPointCode = null;

        RuianMunicipalityEntity? municipality;
        if (advert.LocalityRuian is { } code)
        {
            switch (advert.LocalityRuianLevel)
            {
                case RuianLevelEnum.Address:
                {
                    var point = await ruianService.FindAddressPointByCodeAsync(code, cancellationToken);
                    if (point is null)
                    {
                        return Error("locality_ruian", "Adresní místo s tímto kódem RÚIAN neexistuje.");
                    }

                    if (!CityMatches(advert, point.Municipality))
                    {
                        return CityMismatch();
                    }

                    ApplyAddressPoint(advert, point);
                    return [];
                }
                case RuianLevelEnum.Street:
                {
                    var street = await ruianService.FindStreetByCodeAsync(code, cancellationToken);
                    if (street is null)
                    {
                        return Error("locality_ruian", "Ulice s tímto kódem RÚIAN neexistuje.");
                    }

                    if (!CityMatches(advert, street.Municipality))
                    {
                        return CityMismatch();
                    }

                    municipality = street.Municipality;
                    advert.LocalityStreet = street.Name;
                    break;
                }
                case RuianLevelEnum.Municipality:
                {
                    municipality = await ruianService.FindMunicipalityByCodeAsync(code, cancellationToken);
                    if (municipality is null)
                    {
                        return Error("locality_ruian", "Obec s tímto kódem RÚIAN neexistuje.");
                    }

                    if (!CityMatches(advert, municipality))
                    {
                        return CityMismatch();
                    }

                    break;
                }
                case RuianLevelEnum.District:
                {
                    var district = await ruianService.FindDistrictByCodeAsync(code, cancellationToken);
                    if (district is null)
                    {
                        return Error("locality_ruian", "Okres s tímto kódem RÚIAN neexistuje.");
                    }

                    advert.LocalityDistrictCode = district.Code;
                    municipality = await ruianService.FindMunicipalityByNameAsync(advert.LocalityCity, cancellationToken);
                    if (municipality is not null && municipality.DistrictCode != district.Code)
                    {
                        return Error("locality_city", "Obec neleží v zadaném okrese.");
                    }

                    break;
                }
                default:
                    // TODO: building objects (stavebni objekty) are not imported, so a Building code cannot be checked
                    return Error("locality_ruian_level",
                        "Úroveň RÚIAN Budova není podporována, použijte adresní místo.");
            }
        }
        else
        {
            municipality = await ruianService.FindMunicipalityByNameAsync(advert.LocalityCity, cancellationToken);
        }

        if (municipality is null)
        {
            return [];
        }

        advert.LocalityCity = municipality.Name;
        advert.LocalityMunicipalityCode = municipality.Code;
        advert.LocalityDistrictCode = municipality.DistrictCode;
        if (advert.LocalityRuianLevel is not RuianLevelEnum.Street)
        {
            advert.LocalityRuian = municipality.Code;
            advert.LocalityRuianLevel = RuianLevelEnum.Municipality;
        }

        // the names may pin the address down further than the code did
        if (int.TryParse(advert.LocalityCp, out var houseNumber) && houseNumber > 0)
        {
            var (orientationNumber, orientationLetter) = ParseOrientation(advert.LocalityCo);
            var point = await ruianService.FindAddressPointAsync(municipality.Code, houseNumber,
                advert.LocalityStreet, advert.LocalityCityPart, orientationNumber, orientationLetter,
                cancellationToken);
            if (point is not null)
            {
                ApplyAddressPoint(advert, point);
                return [];
            }
        }

        if (advert.LocalityRuianLevel is not RuianLevelEnum.Street && !string.IsNullOrWhiteSpace(advert.LocalityStreet))
        {
            var street = await ruianService.FindStreetByNameAsync(municipality.Code, advert.LocalityStreet,
                cancellationToken);
            if (street is not null)
            {
                advert.LocalityStreet = street.Name;
                advert.LocalityRuian = street.Code;
                advert.LocalityRuianLevel = RuianLevelEnum.Street;
            }
        }

        return [];
    }

    /// <summary>Copies the address point into the advert; sender-supplied coordinates are kept.</summary>
    private static void ApplyAddressPoint(SrealityAdvertEntity advert, RuianAddressPointEntity point)
    {
        advert.LocalityCity = point.Municipality.Name;
        advert.LocalityCityPart = point.MunicipalityPart.Name;
        advert.LocalityStreet = point.Street?.Name;
        advert.LocalityCp = point.HouseNumberType == HouseNumberTypeEnum.Descriptive
            ? point.HouseNumber.ToString()
            : null;
        advert.LocalityCo = point.OrientationNumber is { } number ? $"{number}{point.OrientationLetter}" : null;
        advert.LocalityLatitude ??= point.Latitude;
        advert.LocalityLongitude ??= point.Longitude;
        advert.LocalityRuian = point.Code;
        advert.LocalityRuianLevel = RuianLevelEnum.Address;
        advert.LocalityAddressPointCode = point.Code;
        advert.LocalityMunicipalityCode = point.MunicipalityCode;
        advert.LocalityDistrictCode = point.Municipality.DistrictCode;
    }

    private static bool CityMatches(SrealityAdvertEntity advert, RuianMunicipalityEntity municipality)
        => RuianNames.Normalize(advert.LocalityCity) == municipality.SearchName;

    private static Dictionary<string, string[]> CityMismatch()
        => Error("locality_city", "Obec neodpovídá zadanému kódu RÚIAN.");

    private static Dictionary<string, string[]> Error(string field, string message)
        => new() { [field] = [message] };

    /// <summary>Splits an orientation number such as "12a" into its number and letter; both null when absent.</summary>
    private static (int? Number, string? Letter) ParseOrientation(string? orientation)
    {
        if (orientation is null)
        {
            return (null, null);
        }

        var match = OrientationRegex().Match(orientation.Trim());
        if (!match.Success)
        {
            return (null, null);
        }

        var letter = match.Groups[2].Value;
        return (int.Parse(match.Groups[1].Value), letter.Length == 0 ? null : letter);
    }

    [GeneratedRegex(@"^(\d+)\s*([A-Za-z]?)$")]
    private static partial Regex OrientationRegex();
}
