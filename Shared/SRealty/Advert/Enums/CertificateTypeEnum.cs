using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Version of the standard an energy certificate was issued under. Not referenced by the advert model yet.</summary>
public enum CertificateTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Standard 2013", DisplayNameEn = "Standard 2013")]
    Standard2013 = 1,

    [LocalizedDisplayName(DisplayNameCz = "Standard 2020", DisplayNameEn = "Standard 2020")]
    Standard2020 = 2
}