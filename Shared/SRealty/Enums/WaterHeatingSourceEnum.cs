using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum WaterHeatingSourceEnum
{
    [SRealtyEnums(DisplayNameCz = "Plynový kondenzační kotel", DisplayNameEn = "Gas condensing boiler")]
    GasCondensingBoiler = 1,

    [SRealtyEnums(DisplayNameCz = "Plynový kotel", DisplayNameEn = "Gas boiler")]
    GasBoiler = 2,

    [SRealtyEnums(DisplayNameCz = "Elektro kotel", DisplayNameEn = "Electric boiler")]
    ElectricBoiler = 3,

    [SRealtyEnums(DisplayNameCz = "Tepelné čerpadlo", DisplayNameEn = "Heat pump")]
    HeatPump = 4,

    [SRealtyEnums(DisplayNameCz = "Plynová kamna", DisplayNameEn = "Gas heater")]
    GasHeater = 5,

    [SRealtyEnums(DisplayNameCz = "Kotel na tuhá paliva", DisplayNameEn = "Solid fuel boiler")]
    SolidFuelBoiler = 6,

    [SRealtyEnums(DisplayNameCz = "Bojler - elektro", DisplayNameEn = "Electric water heater")]
    ElectricWaterHeater = 7,

    [SRealtyEnums(DisplayNameCz = "Bojler - plyn", DisplayNameEn = "Gas water heater")]
    GasWaterHeater = 8,

    [SRealtyEnums(DisplayNameCz = "Průtokový ohřívač", DisplayNameEn = "Tankless water heater")]
    TanklessWaterHeater = 9,

    [SRealtyEnums(DisplayNameCz = "Centrální dálkový ohřev", DisplayNameEn = "District heating")]
    DistrictHeating = 10,

    [SRealtyEnums(DisplayNameCz = "Jiné", DisplayNameEn = "Other")]
    Other = 11
}