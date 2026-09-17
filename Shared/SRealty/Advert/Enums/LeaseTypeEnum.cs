using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Whether the tenant signs a lease with the owner or a sublease with an existing tenant.</summary>
public enum LeaseTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Nájem", DisplayNameEn = "Lease")]
    [ValidForType<AdvertFunctionEnum>(AdvertFunctionEnum.Rent)]
    Lease = 1,

    [LocalizedDisplayName(DisplayNameCz = "Podnájem", DisplayNameEn = "Sublease")]
    [ValidForType<AdvertFunctionEnum>(AdvertFunctionEnum.Rent)]
    Sublease = 2
}
