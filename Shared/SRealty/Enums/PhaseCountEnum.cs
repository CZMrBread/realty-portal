using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum PhaseCountEnum
{
    [SRealtyEnums(DisplayNameCz = "Jednofázové", DisplayNameEn = "Single")]
    Single = 1,

    [SRealtyEnums(DisplayNameCz = "Třífázové", DisplayNameEn = "Three")]
    Three = 2
}