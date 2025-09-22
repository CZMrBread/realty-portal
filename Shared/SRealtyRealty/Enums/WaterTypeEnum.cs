namespace Shared.SRealtyRealty.Enums;

public enum WaterTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Vlastní zdroj", DisplayNameEn = "Local source")]
    LocalSource = 1,

    [SRealtyEnums(DisplayNameCz = "Veřejný vodovod", DisplayNameEn = "Public water")]
    PublicWater = 2,

    [SRealtyEnums(DisplayNameCz = "Studna", DisplayNameEn = "Well")]
    Well = 4
}