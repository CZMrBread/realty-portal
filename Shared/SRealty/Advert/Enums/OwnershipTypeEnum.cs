using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Form of ownership being transferred.</summary>
public enum OwnershipTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Osobní", DisplayNameEn = "Personal")]
    Personal = 1,

    [LocalizedDisplayName(DisplayNameCz = "Družstevní", DisplayNameEn = "Cooperative")]
    Cooperative = 2,

    [LocalizedDisplayName(DisplayNameCz = "Státní nebo obecní", DisplayNameEn = "State or municipal")]
    StateOrMunicipal = 3
}