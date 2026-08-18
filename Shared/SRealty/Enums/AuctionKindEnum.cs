using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum AuctionKindEnum
{
    [SRealtyEnums(DisplayNameCz = "Nedobrovolný", DisplayNameEn = "Involuntary")]
    Involuntary = 1,

    [SRealtyEnums(DisplayNameCz = "Dobrovolný", DisplayNameEn = "Voluntary")]
    Voluntary = 2,

    [SRealtyEnums(DisplayNameCz = "Exekuce", DisplayNameEn = "Enforcement")]
    Enforcement = 3,

    [SRealtyEnums(DisplayNameCz = "Veřejná dražba", DisplayNameEn = "Public auction")]
    PublicAuction = 4
}