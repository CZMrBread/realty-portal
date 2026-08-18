using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum AdvertTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Byt", DisplayNameEn = "Flat", DescriptionCz = "Bytová jednotka v bytovém domě",
        DescriptionEn = "Apartment unit in residential building", Icon = "bi-house-door")]
    Flat = 1,

    [SRealtyEnums(DisplayNameCz = "Dům", DisplayNameEn = "House", DescriptionCz = "Rodinný nebo bytový dům",
        DescriptionEn = "Family or residential house", Icon = "bi-house")]
    House = 2,

    [SRealtyEnums(DisplayNameCz = "Pozemek", DisplayNameEn = "Land", DescriptionCz = "Stavební nebo zemědělský pozemek",
        DescriptionEn = "Building or agricultural land", Icon = "bi-tree")]
    Land = 3,

    [SRealtyEnums(DisplayNameCz = "Komerční", DisplayNameEn = "Commercial",
        DescriptionCz = "Komerční nemovitost pro podnikání", DescriptionEn = "Commercial property for business",
        Icon = "bi-building")]
    Commercial = 4,

    [SRealtyEnums(DisplayNameCz = "Ostatní", DisplayNameEn = "Other", DescriptionCz = "Ostatní typy nemovitostí",
        DescriptionEn = "Other property types", Icon = "bi-collection")]
    Other = 5
}