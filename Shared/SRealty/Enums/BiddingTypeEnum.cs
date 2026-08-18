using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum BiddingTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Anglická", DisplayNameEn = "English")]
    English = 1,

    [SRealtyEnums(DisplayNameCz = "Holandská", DisplayNameEn = "Dutch")]
    Dutch = 2
}