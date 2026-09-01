using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Direction the bidding moves in: English raises the price, Dutch lowers it.</summary>
public enum BiddingTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Anglická", DisplayNameEn = "English")]
    English = 1,

    [LocalizedDisplayName(DisplayNameCz = "Holandská", DisplayNameEn = "Dutch")]
    Dutch = 2
}