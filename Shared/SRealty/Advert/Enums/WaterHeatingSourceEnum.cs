using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Appliance that heats the domestic hot water, which may differ from the space heating source.</summary>
public enum WaterHeatingSourceEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Plynový kondenzační kotel", DisplayNameEn = "Gas condensing boiler")]
    GasCondensingBoiler = 1,

    [LocalizedDisplayName(DisplayNameCz = "Plynový kotel", DisplayNameEn = "Gas boiler")]
    GasBoiler = 2,

    [LocalizedDisplayName(DisplayNameCz = "Elektro kotel", DisplayNameEn = "Electric boiler")]
    ElectricBoiler = 3,

    [LocalizedDisplayName(DisplayNameCz = "Tepelné čerpadlo", DisplayNameEn = "Heat pump")]
    HeatPump = 4,

    [LocalizedDisplayName(DisplayNameCz = "Plynová kamna", DisplayNameEn = "Gas heater")]
    GasHeater = 5,

    [LocalizedDisplayName(DisplayNameCz = "Kotel na tuhá paliva", DisplayNameEn = "Solid fuel boiler")]
    SolidFuelBoiler = 6,

    [LocalizedDisplayName(DisplayNameCz = "Bojler - elektro", DisplayNameEn = "Electric water heater")]
    ElectricWaterHeater = 7,

    [LocalizedDisplayName(DisplayNameCz = "Bojler - plyn", DisplayNameEn = "Gas water heater")]
    GasWaterHeater = 8,

    [LocalizedDisplayName(DisplayNameCz = "Průtokový ohřívač", DisplayNameEn = "Tankless water heater")]
    TanklessWaterHeater = 9,

    [LocalizedDisplayName(DisplayNameCz = "Centrální dálkový ohřev", DisplayNameEn = "District heating")]
    DistrictHeating = 10,

    [LocalizedDisplayName(DisplayNameCz = "Jiné", DisplayNameEn = "Other")]
    Other = 11
}