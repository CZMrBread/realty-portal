namespace Shared.SRealtyRealty.Enums;

public enum OwnershipTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Osobní", DisplayNameEn = "Personal")]
    Personal = 1,

    [SRealtyEnums(DisplayNameCz = "Družstevní", DisplayNameEn = "Cooperative")]
    Cooperative = 2,

    [SRealtyEnums(DisplayNameCz = "Státní nebo obecní", DisplayNameEn = "State or municipal")]
    StateOrMunicipal = 3
}