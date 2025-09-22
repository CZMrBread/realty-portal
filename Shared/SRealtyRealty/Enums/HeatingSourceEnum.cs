namespace Shared.SRealtyRealty.Enums;

public enum HeatingSourceEnum
{
    [SRealtyEnums(DisplayNameCz = "Plynový kotel", DisplayNameEn = "Gas boiler")]
    GasBoiler = 1,

    [SRealtyEnums(DisplayNameCz = "Elektrický kotel", DisplayNameEn = "Electric boiler")]
    ElectricBoiler = 2,

    [SRealtyEnums(DisplayNameCz = "Tepelné čerpadlo", DisplayNameEn = "Heat pump")]
    HeatPump = 3
}