namespace Shared.SRealtyRealty.Enums;

public enum TransportTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Vlak", DisplayNameEn = "Train")]
    Train = 1,

    [SRealtyEnums(DisplayNameCz = "Dálnice", DisplayNameEn = "Highway")]
    Highway = 2,

    [SRealtyEnums(DisplayNameCz = "Silnice", DisplayNameEn = "Road")]
    Road = 3,

    [SRealtyEnums(DisplayNameCz = "MHD", DisplayNameEn = "Public transport")]
    PublicTransport = 4,

    [SRealtyEnums(DisplayNameCz = "Autobus", DisplayNameEn = "Bus")]
    Bus = 5
}