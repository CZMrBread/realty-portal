namespace Shared.SRealtyRealty.Enums;

public enum RoadTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Betonová", DisplayNameEn = "Concrete")]
    Concrete = 1,

    [SRealtyEnums(DisplayNameCz = "Dlážděná", DisplayNameEn = "Paved")]
    Paved = 2,

    [SRealtyEnums(DisplayNameCz = "Asfaltová", DisplayNameEn = "Asphalt")]
    Asphalt = 3,

    [SRealtyEnums(DisplayNameCz = "Nezpevněná", DisplayNameEn = "Unpaved")]
    Unpaved = 4,

    [SRealtyEnums(DisplayNameCz = "Zpevněná", DisplayNameEn = "Hardened")]
    Hardened = 5,

    [SRealtyEnums(DisplayNameCz = "Štěrková", DisplayNameEn = "Gravel")]
    Gravel = 6,

    [SRealtyEnums(DisplayNameCz = "Kamenitá", DisplayNameEn = "Crushed stone")]
    CrushedStone = 7,

    [SRealtyEnums(DisplayNameCz = "Bez přístupové cesty", DisplayNameEn = "No access road")]
    NoAccessRoad = 8
}