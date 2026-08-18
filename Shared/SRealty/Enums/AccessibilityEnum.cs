using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum AccessibilityEnum
{
    [SRealtyEnums(DisplayNameCz = "Ano", DisplayNameEn = "Yes")]
    Yes = 1,

    [SRealtyEnums(DisplayNameCz = "Ne", DisplayNameEn = "No")]
    No = 2,

    [SRealtyEnums(DisplayNameCz = "Částečně", DisplayNameEn = "Partial")]
    Partial = 3
}