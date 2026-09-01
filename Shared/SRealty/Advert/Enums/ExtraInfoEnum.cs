using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Status badge of a reserved or already sold advert.</summary>
public enum ExtraInfoEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Rezervováno", DisplayNameEn = "Reserved")]
    Reserved = 1,

    [LocalizedDisplayName(DisplayNameCz = "Prodáno", DisplayNameEn = "Sold")]
    Sold = 2
}