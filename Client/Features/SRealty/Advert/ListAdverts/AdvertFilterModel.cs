using Shared.SRealty.Advert.Enums;
using Shared.SRealty.Advert.ListAdverts;

namespace Client.Features.SRealty.Advert.ListAdverts;

/// <summary>Mutable twin of <see cref="AdvertFilter"/> that the search form binds to.</summary>
public sealed class AdvertFilterModel
{
    public AdvertFunctionEnum? AdvertFunction { get; set; }
    public AdvertTypeEnum? AdvertType { get; set; }
    public ICollection<AdvertSubtypeEnum>? AdvertSubtypes { get; set; }
    public string? LocalityCity { get; set; }
    public decimal? PriceFrom { get; set; }
    public decimal? PriceTo { get; set; }
    public int? AreaFrom { get; set; }
    public int? AreaTo { get; set; }
    public string? Search { get; set; }

    /// <summary>The filter as the API takes it.</summary>
    public AdvertFilter ToFilter() => new()
    {
        AdvertFunction = AdvertFunction,
        AdvertType = AdvertType,
        AdvertSubtypes = AdvertSubtypes is { Count: > 0 } subtypes ? subtypes.ToArray() : null,
        LocalityCity = LocalityCity,
        PriceFrom = PriceFrom,
        PriceTo = PriceTo,
        AreaFrom = AreaFrom,
        AreaTo = AreaTo,
        Search = Search
    };

    /// <summary>Puts every criterion back to "no restriction".</summary>
    public void Clear()
    {
        AdvertFunction = null;
        AdvertType = null;
        AdvertSubtypes = null;
        LocalityCity = null;
        PriceFrom = null;
        PriceTo = null;
        AreaFrom = null;
        AreaTo = null;
        Search = null;
    }
}
