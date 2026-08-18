using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum TelecommunicationTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Telefon", DisplayNameEn = "Phone")]
    Phone = 1,

    [SRealtyEnums(DisplayNameCz = "Internet", DisplayNameEn = "Internet")]
    Internet = 2,

    [SRealtyEnums(DisplayNameCz = "Satelit", DisplayNameEn = "Satellite")]
    Satellite = 3,

    [SRealtyEnums(DisplayNameCz = "Kabelová televize", DisplayNameEn = "Cable TV")]
    CableTV = 4,

    [SRealtyEnums(DisplayNameCz = "Kabelová distribuce", DisplayNameEn = "Cable distribution")]
    CableDistribution = 5,

    [SRealtyEnums(DisplayNameCz = "Ostatní", DisplayNameEn = "Other")]
    Other = 6
}