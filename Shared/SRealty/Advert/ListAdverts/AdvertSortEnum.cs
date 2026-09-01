using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.ListAdverts;

/// <summary>Order a list of adverts is handed back in.</summary>
public enum AdvertSortEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Nejnovější", DisplayNameEn = "Newest")]
    Newest = 1,

    [LocalizedDisplayName(DisplayNameCz = "Naposledy upravené", DisplayNameEn = "Recently updated")]
    RecentlyUpdated = 2,

    [LocalizedDisplayName(DisplayNameCz = "Nejlevnější", DisplayNameEn = "Cheapest")]
    PriceAscending = 3,

    [LocalizedDisplayName(DisplayNameCz = "Nejdražší", DisplayNameEn = "Most expensive")]
    PriceDescending = 4,

    /// <summary>Compares the area chosen by advert type, as in AdvertFilter.AreaFrom.</summary>
    [LocalizedDisplayName(DisplayNameCz = "Nejmenší plocha", DisplayNameEn = "Smallest area")]
    AreaAscending = 5,

    [LocalizedDisplayName(DisplayNameCz = "Největší plocha", DisplayNameEn = "Largest area")]
    AreaDescending = 6
}
