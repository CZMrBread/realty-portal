using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum RuianLevelEnum
{
    [SRealtyEnums(DisplayNameCz = "Okres", DisplayNameEn = "District")]
    District = 1,

    [SRealtyEnums(DisplayNameCz = "Obec", DisplayNameEn = "Municipality")]
    Municipality = 3,

    [SRealtyEnums(DisplayNameCz = "Ulice", DisplayNameEn = "Street")]
    Street = 7,

    [SRealtyEnums(DisplayNameCz = "Budova", DisplayNameEn = "Building")]
    Building = 9,

    [SRealtyEnums(DisplayNameCz = "Adresa", DisplayNameEn = "Address")]
    Address = 11
}