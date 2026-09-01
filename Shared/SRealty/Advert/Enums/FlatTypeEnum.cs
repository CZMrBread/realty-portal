using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Layout of a flat; superseded by FlatClassEnum and unused by the advert model.</summary>
public enum FlatTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Standardní", DisplayNameEn = "Standard")]
    Standard = 1,

    [LocalizedDisplayName(DisplayNameCz = "Mezonet", DisplayNameEn = "Maisonette")]
    Maisonette = 2,

    [LocalizedDisplayName(DisplayNameCz = "Loft", DisplayNameEn = "Loft")]
    Loft = 3,

    [LocalizedDisplayName(DisplayNameCz = "Podkroví", DisplayNameEn = "Attic")]
    Attic = 4
}