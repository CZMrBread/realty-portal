namespace Shared.SRealtyRealty.Enums;

public enum HeatingElementEnum
{
    [SRealtyEnums(DisplayNameCz = "Radiátory", DisplayNameEn = "Radiators")]
    Radiators = 1,

    [SRealtyEnums(DisplayNameCz = "Podlahové topní", DisplayNameEn = "Floor heating")]
    FloorHeating = 2,

    [SRealtyEnums(DisplayNameCz = "Klimatizace", DisplayNameEn = "Air conditioning")]
    AirConditioning = 3
}