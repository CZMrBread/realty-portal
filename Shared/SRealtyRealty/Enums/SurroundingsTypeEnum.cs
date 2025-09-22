namespace Shared.SRealtyRealty.Enums;

public enum SurroundingsTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Bydlení", DisplayNameEn = "Residential")]
    Residential = 1,

    [SRealtyEnums(DisplayNameCz = "Bydlení a kanceláře", DisplayNameEn = "Residential and offices")]
    ResidentialAndOffices = 2,

    [SRealtyEnums(DisplayNameCz = "Komerční", DisplayNameEn = "Commercial")]
    Commercial = 3,

    [SRealtyEnums(DisplayNameCz = "Administrativní", DisplayNameEn = "Administrative")]
    Administrative = 4,

    [SRealtyEnums(DisplayNameCz = "Průmyslová", DisplayNameEn = "Industrial")]
    Industrial = 5,

    [SRealtyEnums(DisplayNameCz = "Venkovská", DisplayNameEn = "Rural")]
    Rural = 6,

    [SRealtyEnums(DisplayNameCz = "Rekreační", DisplayNameEn = "Recreational")]
    Recreational = 7,

    [SRealtyEnums(DisplayNameCz = "Nevyužitá rekreační", DisplayNameEn = "Unused recreational")]
    UnusedRecreational = 8
}