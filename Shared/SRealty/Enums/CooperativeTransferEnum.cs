using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum CooperativeTransferEnum
{
    [SRealtyEnums(DisplayNameCz = "Ano", DisplayNameEn = "Yes")]
    Yes = 1,

    [SRealtyEnums(DisplayNameCz = "Ne", DisplayNameEn = "No")]
    No = 2
}