using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Transport links available near the property.</summary>
public enum TransportTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Vlak", DisplayNameEn = "Train")]
    Train = 1,

    [LocalizedDisplayName(DisplayNameCz = "Dálnice", DisplayNameEn = "Highway")]
    Highway = 2,

    [LocalizedDisplayName(DisplayNameCz = "Silnice", DisplayNameEn = "Road")]
    Road = 3,

    [LocalizedDisplayName(DisplayNameCz = "MHD", DisplayNameEn = "Public transport")]
    PublicTransport = 4,

    [LocalizedDisplayName(DisplayNameCz = "Autobus", DisplayNameEn = "Bus")]
    Bus = 5
}