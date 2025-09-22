namespace Shared.SRealtyRealty.Enums;

public enum HeatingTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Plynové", DisplayNameEn = "Gas")]
    Gas = 1,

    [SRealtyEnums(DisplayNameCz = "Elektrické", DisplayNameEn = "Electric")]
    Electric = 2,

    [SRealtyEnums(DisplayNameCz = "Tuhlé palivo", DisplayNameEn = "Solid")]
    Solid = 3,

    [SRealtyEnums(DisplayNameCz = "Dálkové", DisplayNameEn = "Remote")]
    Remote = 4,

    [SRealtyEnums(DisplayNameCz = "Ostatní", DisplayNameEn = "Other")]
    Other = 5
}