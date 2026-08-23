using Shared.SRealty.Advert.Enums;

namespace Shared.SRealty.Advert.ListAdverts;

/// <summary>Criteria for narrowing down a list of adverts. A property left null puts no restriction on the result.</summary>
public sealed record AdvertFilter
{
    public AdvertFunctionEnum? AdvertFunction { get; init; }
    public AdvertTypeEnum? AdvertType { get; init; }
    public AdvertSubtypeEnum? AdvertSubtype { get; init; }
    public string? LocalityCity { get; init; }
    /// <summary>Lower bound of the price range, inclusive.</summary>
    public decimal? PriceFrom { get; init; }
    /// <summary>Upper bound of the price range, inclusive.</summary>
    public decimal? PriceTo { get; init; }
}
