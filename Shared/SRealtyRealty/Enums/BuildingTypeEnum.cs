namespace Shared.SRealtyRealty.Enums;

public enum BuildingTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Dřevostavba", DisplayNameEn = "Wood Frame",
        DescriptionCz = "Stavba z dřevěné konstrukce", DescriptionEn = "Wooden construction building",
        Icon = "bi-tree-fill")]
    WoodFrame = 1,

    [SRealtyEnums(DisplayNameCz = "Cihlová", DisplayNameEn = "Brick", DescriptionCz = "Stavba z cihelných bloků",
        DescriptionEn = "Brick construction building", Icon = "bi-bricks")]
    Brick = 2,

    [SRealtyEnums(DisplayNameCz = "Kamenná", DisplayNameEn = "Stone", DescriptionCz = "Stavba z kamene",
        DescriptionEn = "Stone construction building", Icon = "bi-gem")]
    Stone = 3,

    [SRealtyEnums(DisplayNameCz = "Panelová", DisplayNameEn = "Panel", DescriptionCz = "Panelová konstrukce",
        DescriptionEn = "Panel construction building", Icon = "bi-grid-3x3")]
    Panel = 4,

    [SRealtyEnums(DisplayNameCz = "Skeletová", DisplayNameEn = "Skeleton",
        DescriptionCz = "Železobetonová skeletová konstrukce", DescriptionEn = "Reinforced concrete frame construction",
        Icon = "bi-building-gear")]
    Skeleton = 5,

    [SRealtyEnums(DisplayNameCz = "Smíšená", DisplayNameEn = "Mixed",
        DescriptionCz = "Kombinace různých stavebních materiálů",
        DescriptionEn = "Combination of various construction materials", Icon = "bi-layers")]
    Mixed = 6
}