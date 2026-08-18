using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum GasTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Individuální", DisplayNameEn = "Individual")]
    Individual = 1,

    [SRealtyEnums(DisplayNameCz = "Plynovod", DisplayNameEn = "Pipeline")]
    Pipeline = 2
}