using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum CertificateTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Standard 2013", DisplayNameEn = "Standard 2013")]
    Standard2013 = 1,

    [SRealtyEnums(DisplayNameCz = "Standard 2020", DisplayNameEn = "Standard 2020")]
    Standard2020 = 2
}