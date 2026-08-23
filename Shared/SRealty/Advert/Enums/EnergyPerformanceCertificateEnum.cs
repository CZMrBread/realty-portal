using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Czech act the energy performance certificate was issued under, which determines how its rating is to be read.</summary>
public enum EnergyPerformanceCertificateEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Zákon 148/2007 Sb.", DisplayNameEn = "Law 148/2007 Coll.")]
    Law148_2007 = 1,

    [LocalizedDisplayName(DisplayNameCz = "Zákon 78/2013 Sb.", DisplayNameEn = "Law 78/2013 Coll.")]
    Law78_2013 = 2,

    [LocalizedDisplayName(DisplayNameCz = "Zákon 264/2020 Sb.", DisplayNameEn = "Law 264/2020 Coll.")]
    Law264_2020 = 3
}