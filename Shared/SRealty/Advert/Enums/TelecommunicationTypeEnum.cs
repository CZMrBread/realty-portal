using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Telecommunication services connected to the property.</summary>
public enum TelecommunicationTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Telefon", DisplayNameEn = "Phone")]
    Phone = 1,

    [LocalizedDisplayName(DisplayNameCz = "Internet", DisplayNameEn = "Internet")]
    Internet = 2,

    [LocalizedDisplayName(DisplayNameCz = "Satelit", DisplayNameEn = "Satellite")]
    Satellite = 3,

    [LocalizedDisplayName(DisplayNameCz = "Kabelová televize", DisplayNameEn = "Cable TV")]
    CableTV = 4,

    [LocalizedDisplayName(DisplayNameCz = "Kabelová distribuce", DisplayNameEn = "Cable distribution")]
    CableDistribution = 5,

    [LocalizedDisplayName(DisplayNameCz = "Ostatní", DisplayNameEn = "Other")]
    Other = 6
}