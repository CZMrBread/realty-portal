using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Whether a cooperative flat can be transferred to personal ownership; unused by the advert model.</summary>
public enum CooperativeTransferEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Ano", DisplayNameEn = "Yes")]
    Yes = 1,

    [LocalizedDisplayName(DisplayNameCz = "Ne", DisplayNameEn = "No")]
    No = 2
}