using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum AdvertRoomCountEnum
{
    [SRealtyEnums(DisplayNameCz = "1 pokoj", DisplayNameEn = "1 room")]
    OneRoom = 1,

    [SRealtyEnums(DisplayNameCz = "2 pokoje", DisplayNameEn = "2 rooms")]
    TwoRooms = 2,

    [SRealtyEnums(DisplayNameCz = "3 pokoje", DisplayNameEn = "3 rooms")]
    ThreeRooms = 3,

    [SRealtyEnums(DisplayNameCz = "4 pokoje", DisplayNameEn = "4 rooms")]
    FourRooms = 4,

    [SRealtyEnums(DisplayNameCz = "5 a více pokojů", DisplayNameEn = "5 and more rooms")]
    FiveAndMoreRooms = 5,

    [SRealtyEnums(DisplayNameCz = "Atypický", DisplayNameEn = "Atypical")]
    Atypical = 6
}