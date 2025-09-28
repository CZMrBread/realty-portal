namespace Shared.SRealtyRealty.Enums;

public enum SewerageTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Veřejná kanalizace", DisplayNameEn = "Public sewer")]
    PublicSewer = 1,

    [SRealtyEnums(DisplayNameCz = "Čistička odpadních vod pro celý objekt", DisplayNameEn = "Object treatment plant")]
    ObjectTreatmentPlant = 2,
    
    [SRealtyEnums(DisplayNameCz = "Septik", DisplayNameEn = "Septic tank")]
    SepticTank = 3,
    
    [SRealtyEnums(DisplayNameCz = "Jímka", DisplayNameEn = "Cesspool")]
    Cesspool = 4,
    
    [SRealtyEnums(DisplayNameCz = "Trativod", DisplayNameEn = "Drainage")]
    Drainage = 5
}