using Shared.SRealty.Advert.Enums;

namespace Shared.SRealty.Advert.ListAdverts;

/// <summary>
/// Criteria narrowing a list of adverts; null means no restriction, an array matches any of its members.
/// Bound from the query string, so members must be scalars or arrays of scalars.
/// </summary>
public sealed record AdvertFilter
{
    public AdvertFunctionEnum? AdvertFunction { get; init; }
    public AdvertTypeEnum? AdvertType { get; init; }

    /// <summary>Layouts to match, such as 2+kk or 3+1.</summary>
    public AdvertSubtypeEnum[]? AdvertSubtypes { get; init; }

    /// <summary>Town as the agency wrote it, matched as a case-insensitive prefix.</summary>
    public string? LocalityCity { get; init; }

    /// <summary>RUIAN code of a region (kraj).</summary>
    public int? RegionCode { get; init; }

    /// <summary>RUIAN code of a district (okres).</summary>
    public int? DistrictCode { get; init; }

    /// <summary>RUIAN code of a municipality (obec).</summary>
    public int? MunicipalityCode { get; init; }

    /// <summary>Lower bound of the price range, inclusive.</summary>
    public decimal? PriceFrom { get; init; }

    /// <summary>Upper bound of the price range, inclusive.</summary>
    public decimal? PriceTo { get; init; }

    /// <summary>Inclusive lower bound of the area in square metres: estate area for land, else usable area.</summary>
    public int? AreaFrom { get; init; }

    /// <summary>Upper bound of the area in square metres, inclusive; see <see cref="AreaFrom"/>.</summary>
    public int? AreaTo { get; init; }

    /// <summary>Building conditions to match.</summary>
    public BuildingConditionEnum[]? BuildingConditions { get; init; }

    /// <summary>Words to find in the description and address; all must appear, accents ignored.</summary>
    public string? Search { get; init; }

    /// <summary>Agency the adverts are published under.</summary>
    public Guid? RealtyAgencyId { get; init; }

    /// <summary>Agent selling the adverts.</summary>
    public Guid? SellerId { get; init; }
}
