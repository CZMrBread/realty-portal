using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum ProtectionEnum
{
    [SRealtyEnums(DisplayNameCz = "Ochranné pásmo", DisplayNameEn = "Protection zone")]
    ProtectionZone = 1,

    [SRealtyEnums(DisplayNameCz = "Národní park", DisplayNameEn = "National park")]
    NationalPark = 2,

    [SRealtyEnums(DisplayNameCz = "Chráněná krajinná oblast", DisplayNameEn = "Protected landscape area")]
    ProtectedLandscapeArea = 3,

    [SRealtyEnums(DisplayNameCz = "Památková zóna", DisplayNameEn = "Monument zone")]
    MonumentZone = 4,

    [SRealtyEnums(DisplayNameCz = "Památková rezervace", DisplayNameEn = "Monument reservation")]
    MonumentReservation = 5,

    [SRealtyEnums(DisplayNameCz = "Kulturní památka", DisplayNameEn = "Cultural monument")]
    CulturalMonument = 6,

    [SRealtyEnums(DisplayNameCz = "Národní kulturní památka", DisplayNameEn = "National cultural monument")]
    NationalCulturalMonument = 7
}