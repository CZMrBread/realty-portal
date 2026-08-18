using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum EnergyPerformanceCertificateEnum
{
    [SRealtyEnums(DisplayNameCz = "Zákon 148/2007 Sb.", DisplayNameEn = "Law 148/2007 Coll.")]
    Law148_2007 = 1,

    [SRealtyEnums(DisplayNameCz = "Zákon 78/2013 Sb.", DisplayNameEn = "Law 78/2013 Coll.")]
    Law78_2013 = 2,

    [SRealtyEnums(DisplayNameCz = "Zákon 264/2020 Sb.", DisplayNameEn = "Law 264/2020 Coll.")]
    Law264_2020 = 3
}