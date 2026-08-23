using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Status badge for an advert that is no longer freely available: reserved or already sold.</summary>
public enum ExtraInfoEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Rezervováno", DisplayNameEn = "Reserved")]
    Reserved = 1,

    [LocalizedDisplayName(DisplayNameCz = "Prodáno", DisplayNameEn = "Sold")]
    Sold = 2
}