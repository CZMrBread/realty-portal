using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Layout class of a flat, such as a maisonette or an attic conversion.</summary>
public enum FlatClassEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Mezonet", DisplayNameEn = "Maisonette")]
    Maisonette = 1,

    [LocalizedDisplayName(DisplayNameCz = "Loft", DisplayNameEn = "Loft")]
    Loft = 2,

    [LocalizedDisplayName(DisplayNameCz = "Podkroví", DisplayNameEn = "Attic")]
    Attic = 3,

    [LocalizedDisplayName(DisplayNameCz = "Jednopodlažní", DisplayNameEn = "Single-story")]
    SingleStory = 4
}