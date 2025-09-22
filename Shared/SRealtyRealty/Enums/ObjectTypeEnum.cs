namespace Shared.SRealtyRealty.Enums;

public enum ObjectTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Jednopodlažní", DisplayNameEn = "Single floor")]
    SingleFloor = 1,

    [SRealtyEnums(DisplayNameCz = "Vícepodlažní", DisplayNameEn = "Multi floor")]
    MultiFloor = 2
}