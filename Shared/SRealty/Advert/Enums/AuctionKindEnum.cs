using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Legal form of the auction the property is sold through.</summary>
public enum AuctionKindEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Nedobrovolný", DisplayNameEn = "Involuntary")]
    Involuntary = 1,

    [LocalizedDisplayName(DisplayNameCz = "Dobrovolný", DisplayNameEn = "Voluntary")]
    Voluntary = 2,

    [LocalizedDisplayName(DisplayNameCz = "Exekuce", DisplayNameEn = "Enforcement")]
    Enforcement = 3,

    [LocalizedDisplayName(DisplayNameCz = "Veřejná dražba", DisplayNameEn = "Public auction")]
    PublicAuction = 4
}