using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Heating arrangement, combining whether the source is local or central with the fuel it burns.</summary>
public enum HeatingEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Lokální - plyn", DisplayNameEn = "Local - gas")]
    LocalGas = 1,

    [LocalizedDisplayName(DisplayNameCz = "Lokální - tuhá paliva", DisplayNameEn = "Local - solid fuel")]
    LocalSolidFuel = 2,

    [LocalizedDisplayName(DisplayNameCz = "Lokální - elektřina", DisplayNameEn = "Local - electricity")]
    LocalElectric = 3,

    [LocalizedDisplayName(DisplayNameCz = "Ústřední - plyn", DisplayNameEn = "Central - gas")]
    CentralGas = 4,

    [LocalizedDisplayName(DisplayNameCz = "Ústřední - tuhá paliva", DisplayNameEn = "Central - solid fuel")]
    CentralSolidFuel = 5,

    [LocalizedDisplayName(DisplayNameCz = "Ústřední - elektřina", DisplayNameEn = "Central - electricity")]
    CentralElectric = 6,

    [LocalizedDisplayName(DisplayNameCz = "Ústřední - dálkové", DisplayNameEn = "Central - remote")]
    CentralRemote = 7,

    [LocalizedDisplayName(DisplayNameCz = "Ostatní", DisplayNameEn = "Other")]
    Other = 8,

    [LocalizedDisplayName(DisplayNameCz = "Podlahové", DisplayNameEn = "Underfloor")]
    Floor = 9
}