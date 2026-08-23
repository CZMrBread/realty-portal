using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Whether a cooperative flat can be transferred into personal ownership. Not referenced by the advert model yet.</summary>
public enum CooperativeTransferEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Ano", DisplayNameEn = "Yes")]
    Yes = 1,

    [LocalizedDisplayName(DisplayNameCz = "Ne", DisplayNameEn = "No")]
    No = 2
}