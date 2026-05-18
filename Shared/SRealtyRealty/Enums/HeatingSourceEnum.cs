namespace Shared.SRealtyRealty.Enums;

public enum HeatingSourceEnum
{
    [SRealtyEnums(DescriptionCz = "WAW", DescriptionEn = "WAW")]
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