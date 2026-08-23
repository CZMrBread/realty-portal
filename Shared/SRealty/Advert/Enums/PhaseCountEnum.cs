using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Whether the electrical installation is single-phase or three-phase.</summary>
public enum PhaseCountEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Jednofázové", DisplayNameEn = "Single")]
    Single = 1,

    [LocalizedDisplayName(DisplayNameCz = "Třífázové", DisplayNameEn = "Three")]
    Three = 2
}