using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>How much furniture is included with the property.</summary>
public enum FurnishingEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Zařízený", DisplayNameEn = "Furnished", DescriptionCz = "Plně zařízený nábytek",
        DescriptionEn = "Fully furnished with furniture", Icon = "bi-house-check")]
    Furnished = 1,

    [LocalizedDisplayName(DisplayNameCz = "Nezařízený", DisplayNameEn = "Unfurnished", DescriptionCz = "Bez nábytku",
        DescriptionEn = "Without furniture", Icon = "bi-house")]
    Unfurnished = 2,

    [LocalizedDisplayName(DisplayNameCz = "Částečně zařízený", DisplayNameEn = "Partially Furnished",
        DescriptionCz = "Částečně vybaven nábytkem", DescriptionEn = "Partially equipped with furniture",
        Icon = "bi-house-dash")]
    PartiallyFurnished = 3
}