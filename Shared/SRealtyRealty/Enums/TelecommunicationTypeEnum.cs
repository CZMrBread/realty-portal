namespace Shared.SRealtyRealty.Enums;

public enum TelecommunicationTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Telefon", DisplayNameEn = "Phone")]
    Phone = 1,

    [SRealtyEnums(DisplayNameCz = "Internet", DisplayNameEn = "Internet")]
    Internet = 2,

    [SRealtyEnums(DisplayNameCz = "Kabelová TV", DisplayNameEn = "Cable TV")]
    CableTV = 3
}