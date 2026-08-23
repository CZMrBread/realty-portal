using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Whether the building has an elevator.</summary>
public enum ElevatorEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Ano", DisplayNameEn = "Yes")]
    Yes = 1,

    [LocalizedDisplayName(DisplayNameCz = "Ne", DisplayNameEn = "No")]
    No = 2
}