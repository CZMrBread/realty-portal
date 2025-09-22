namespace Shared.SRealtyRealty.Enums;

public enum FurnishingEnum
{
    [SRealtyEnums(DisplayNameCz = "Zařízený", DisplayNameEn = "Furnished", DescriptionCz = "Plně zařízený nábytek",
        DescriptionEn = "Fully furnished with furniture", Icon = "bi-house-check")]
    Furnished = 1,

    [SRealtyEnums(DisplayNameCz = "Nezařízený", DisplayNameEn = "Unfurnished", DescriptionCz = "Bez nábytku",
        DescriptionEn = "Without furniture", Icon = "bi-house")]
    Unfurnished = 2,

    [SRealtyEnums(DisplayNameCz = "Částečně zařízený", DisplayNameEn = "Partially Furnished",
        DescriptionCz = "Částečně vybaven nábytkem", DescriptionEn = "Partially equipped with furniture",
        Icon = "bi-house-dash")]
    PartiallyFurnished = 3
}