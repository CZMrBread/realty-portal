using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Whether the property is barrier-free accessible.</summary>
public enum AccessibilityEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Ano", DisplayNameEn = "Yes")]
    Yes = 1,

    [LocalizedDisplayName(DisplayNameCz = "Ne", DisplayNameEn = "No")]
    No = 2,

    [LocalizedDisplayName(DisplayNameCz = "Částečně", DisplayNameEn = "Partial")]
    Partial = 3
}