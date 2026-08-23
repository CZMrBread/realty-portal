using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>How gas is supplied: from an individual tank or from the public pipeline.</summary>
public enum GasTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Individuální", DisplayNameEn = "Individual")]
    Individual = 1,

    [LocalizedDisplayName(DisplayNameCz = "Plynovod", DisplayNameEn = "Pipeline")]
    Pipeline = 2
}