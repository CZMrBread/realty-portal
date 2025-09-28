

namespace Shared.SRealtyRealty.Enums;

public enum FlatClassEnum
{
    [SRealtyEnums(DisplayNameCz = "Mezonet", DisplayNameEn = "Maisonette")]
    Maisonette = 1,

    [SRealtyEnums(DisplayNameCz = "Loft", DisplayNameEn = "Loft")]
    Loft = 2,

    [SRealtyEnums(DisplayNameCz = "Podkroví", DisplayNameEn = "Attic")]
    Attic = 3,

    [SRealtyEnums(DisplayNameCz = "Jednopodlažní", DisplayNameEn = "Single-story")]
    SingleStory = 4
}