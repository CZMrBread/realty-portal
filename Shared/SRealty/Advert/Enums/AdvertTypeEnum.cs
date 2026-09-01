using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Top-level property category; decides which subtypes and conditionally required fields apply.</summary>
public enum AdvertTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Byt", DisplayNameEn = "Flat", DescriptionCz = "Bytová jednotka v bytovém domě",
        DescriptionEn = "Apartment unit in residential building", Icon = "bi-house-door")]
    Flat = 1,

    [LocalizedDisplayName(DisplayNameCz = "Dům", DisplayNameEn = "House", DescriptionCz = "Rodinný nebo bytový dům",
        DescriptionEn = "Family or residential house", Icon = "bi-house")]
    House = 2,

    [LocalizedDisplayName(DisplayNameCz = "Pozemek", DisplayNameEn = "Land", DescriptionCz = "Stavební nebo zemědělský pozemek",
        DescriptionEn = "Building or agricultural land", Icon = "bi-tree")]
    Land = 3,

    [LocalizedDisplayName(DisplayNameCz = "Komerční", DisplayNameEn = "Commercial",
        DescriptionCz = "Komerční nemovitost pro podnikání", DescriptionEn = "Commercial property for business",
        Icon = "bi-building")]
    Commercial = 4,

    [LocalizedDisplayName(DisplayNameCz = "Ostatní", DisplayNameEn = "Other", DescriptionCz = "Ostatní typy nemovitostí",
        DescriptionEn = "Other property types", Icon = "bi-collection")]
    Other = 5
}