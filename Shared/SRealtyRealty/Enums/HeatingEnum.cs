namespace Shared.SRealtyRealty.Enums;

public enum HeatingEnum
{
    [SRealtyEnums(DisplayNameCz = "Lokální - plyn", DisplayNameEn = "Local - gas")]
    LocalGas = 1,
    [SRealtyEnums(DisplayNameCz = "Lokální - tuhá paliva", DisplayNameEn = "Local - solid fuel")]
    LocalSolidFuel = 2,
    [SRealtyEnums(DisplayNameCz = "Lokální - elektřina", DisplayNameEn = "Local - electricity")]
    LocalElectric = 3,
    [SRealtyEnums(DisplayNameCz = "Ústřední - plyn", DisplayNameEn = "Central - gas")]
    CentralGas = 4,
    [SRealtyEnums(DisplayNameCz = "Ústřední - tuhá paliva", DisplayNameEn = "Central - solid fuel")]
    CentralSolidFuel = 5,
    [SRealtyEnums(DisplayNameCz = "Ústřední - elektřina", DisplayNameEn = "Central - electricity")]
    CentralElectric = 6,
    [SRealtyEnums(DisplayNameCz = "Ústřední - dálkové", DisplayNameEn = "Central - remote")]
    CentralRemote = 7,
    [SRealtyEnums(DisplayNameCz = "Ostatní", DisplayNameEn = "Other")]
    Other = 8,
    [SRealtyEnums(DisplayNameCz = "Podlahové", DisplayNameEn = "Underfloor")]
    Floor = 9
}