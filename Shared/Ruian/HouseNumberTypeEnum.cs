using Shared.Shared.Attributes;

namespace Shared.Ruian;

/// <summary>Kind of the house number a RUIAN address point carries.</summary>
public enum HouseNumberTypeEnum
{
    /// <summary>Descriptive number (cislo popisne), unique within the municipality part.</summary>
    [LocalizedDisplayName(DisplayNameCz = "Číslo popisné", DisplayNameEn = "Descriptive number")]
    Descriptive = 1,

    /// <summary>Registration number (cislo evidencni) of a building without a descriptive number.</summary>
    [LocalizedDisplayName(DisplayNameCz = "Číslo evidenční", DisplayNameEn = "Registration number")]
    Registration = 2
}
