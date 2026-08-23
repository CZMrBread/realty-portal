using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Material and construction method of the building.</summary>
public enum BuildingTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Dřevostavba", DisplayNameEn = "Wood Frame",
        DescriptionCz = "Stavba z dřevěné konstrukce", DescriptionEn = "Wooden construction building",
        Icon = "bi-tree-fill")]
    WoodFrame = 1,

    [LocalizedDisplayName(DisplayNameCz = "Cihlová", DisplayNameEn = "Brick", DescriptionCz = "Stavba z cihelných bloků",
        DescriptionEn = "Brick construction building", Icon = "bi-bricks")]
    Brick = 2,

    [LocalizedDisplayName(DisplayNameCz = "Kamenná", DisplayNameEn = "Stone", DescriptionCz = "Stavba z kamene",
        DescriptionEn = "Stone construction building", Icon = "bi-gem")]
    Stone = 3,

    [LocalizedDisplayName(DisplayNameCz = "Montovaná", DisplayNameEn = "Prefabricated",
        DescriptionCz = "Montovaná stavba z prefabrikovaných dílů",
        DescriptionEn = "Prefabricated building from prefabricated parts", Icon = "bi-box-seam")]
    Prefabricated = 4,

    [LocalizedDisplayName(DisplayNameCz = "Panelová", DisplayNameEn = "Panel", DescriptionCz = "Panelová konstrukce",
        DescriptionEn = "Panel construction building", Icon = "bi-grid-3x3")]
    Panel = 5,

    [LocalizedDisplayName(DisplayNameCz = "Skeletová", DisplayNameEn = "Skeleton",
        DescriptionCz = "Železobetonová skeletová konstrukce", DescriptionEn = "Reinforced concrete frame construction",
        Icon = "bi-building-gear")]
    Skeleton = 6,

    [LocalizedDisplayName(DisplayNameCz = "Smíšená", DisplayNameEn = "Mixed",
        DescriptionCz = "Kombinace různých stavebních materiálů",
        DescriptionEn = "Combination of various construction materials", Icon = "bi-layers")]
    Mixed = 7,

    [LocalizedDisplayName(DisplayNameCz = "Modulární", DisplayNameEn = "Modular",
        DescriptionCz = "Stavba z modulů", DescriptionEn = "Building from modules", Icon = "bi-boxes")]
    Modular = 8
}