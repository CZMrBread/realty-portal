namespace Shared.SRealtyRealty.Enums;

public enum WaterHeatingSourceEnum
{
    [SRealtyEnums(DisplayNameCz = "Plynový kotel", DisplayNameEn = "Gas boiler")]
    GasBoiler = 1,

    [SRealtyEnums(DisplayNameCz = "Elektrický kotel", DisplayNameEn = "Electric boiler")]
    ElectricBoiler = 2,

    [SRealtyEnums(DisplayNameCz = "Solární", DisplayNameEn = "Solar")]
    Solar = 3
}