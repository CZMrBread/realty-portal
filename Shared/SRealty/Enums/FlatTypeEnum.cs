using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum FlatTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Standardní", DisplayNameEn = "Standard")]
    Standard = 1,

    [SRealtyEnums(DisplayNameCz = "Mezonet", DisplayNameEn = "Maisonette")]
    Maisonette = 2,

    [SRealtyEnums(DisplayNameCz = "Loft", DisplayNameEn = "Loft")]
    Loft = 3,

    [SRealtyEnums(DisplayNameCz = "Podkroví", DisplayNameEn = "Attic")]
    Attic = 4
}