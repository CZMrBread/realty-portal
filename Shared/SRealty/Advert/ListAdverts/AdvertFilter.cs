using Shared.SRealty.Advert.Enums;

namespace Shared.SRealty.Advert.ListAdverts;

/// <summary>
/// Criteria for narrowing down a list of adverts. A property left null puts no restriction on the result;
/// an array matches any of its members. Bound straight from the query string, so every member has to be a
/// scalar or an array of scalars.
/// </summary>
public sealed record AdvertFilter
{
    public AdvertFunctionEnum? AdvertFunction { get; init; }
    public AdvertTypeEnum? AdvertType { get; init; }

    /// <summary>Layouts to match, such as 2+kk or 3+1. Any of them qualifies.</summary>
    public AdvertSubtypeEnum[]? AdvertSubtypes { get; init; }

    /// <summary>Town as the agency wrote it. Matched as a case-insensitive prefix, unlike the RUIAN codes below, which are exact.</summary>
    public string? LocalityCity { get; init; }

    /// <summary>RUIAN code of a region (kraj); the advert has to lie in a municipality of one of its districts.</summary>
    public int? RegionCode { get; init; }

    /// <summary>RUIAN code of a district (okres).</summary>
    public int? DistrictCode { get; init; }

    /// <summary>RUIAN code of a municipality (obec).</summary>
    public int? MunicipalityCode { get; init; }

    /// <summary>Lower bound of the price range, inclusive.</summary>
    public decimal? PriceFrom { get; init; }

    /// <summary>Upper bound of the price range, inclusive.</summary>
    public decimal? PriceTo { get; init; }

    /// <summary>Lower bound of the area in square metres, inclusive. Which area is meant depends on the advert type: the estate area for land, the usable area for everything else.</summary>
    public int? AreaFrom { get; init; }

    /// <summary>Upper bound of the area in square metres, inclusive. See <see cref="AreaFrom"/> for which area is meant.</summary>
    public int? AreaTo { get; init; }

    /// <summary>States of the building to match. Any of them qualifies.</summary>
    public BuildingConditionEnum[]? BuildingConditions { get; init; }

    /// <summary>Words to look for in the description and the address. Every word has to appear; accents are ignored.</summary>
    public string? Search { get; init; }
}
