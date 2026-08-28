using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Appliance that produces the heat, such as a boiler or a heat pump.</summary>
public enum HeatingSourceEnum
{
    [LocalizedDisplayName(DisplayNameCz = "WAW", DisplayNameEn = "WAW")]
    WAW = 1,

    GasCondensingBoiler = 2,
    GasBoiler = 3,
    ElectricBoiler = 4,
    HeatPump = 5,
    DirectHeater = 6,
    InfraredPanel = 7,
    WoodBurningStove = 8,
    Fireplace = 9,
    SolidFuelBoiler = 10,
    Stove = 11,
    CentralRemote = 12,
    CentralDistrictHeating = 13,
    SteamWithExchanger = 14,
    StorageStove = 15,
    Other = 16,
}
