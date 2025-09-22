namespace Shared.SRealtyRealty.Enums;

public enum SewerageTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Veřejná kanalizace", DisplayNameEn = "Public sewer")]
    PublicSewer = 1,

    [SRealtyEnums(DisplayNameCz = "Žumpa", DisplayNameEn = "Septic")]
    Septic = 2,

    [SRealtyEnums(DisplayNameCz = "Jímka", DisplayNameEn = "Cess pool")]
    CessPool = 3
}