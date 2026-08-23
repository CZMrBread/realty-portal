using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Predominant character of the neighbourhood around the property.</summary>
public enum SurroundingsTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Bydlení", DisplayNameEn = "Residential")]
    Residential = 1,

    [LocalizedDisplayName(DisplayNameCz = "Bydlení a kanceláře", DisplayNameEn = "Residential and offices")]
    ResidentialAndOffices = 2,

    [LocalizedDisplayName(DisplayNameCz = "Komerční", DisplayNameEn = "Commercial")]
    Commercial = 3,

    [LocalizedDisplayName(DisplayNameCz = "Administrativní", DisplayNameEn = "Administrative")]
    Administrative = 4,

    [LocalizedDisplayName(DisplayNameCz = "Průmyslová", DisplayNameEn = "Industrial")]
    Industrial = 5,

    [LocalizedDisplayName(DisplayNameCz = "Venkovská", DisplayNameEn = "Rural")]
    Rural = 6,

    [LocalizedDisplayName(DisplayNameCz = "Rekreační", DisplayNameEn = "Recreational")]
    Recreational = 7,

    [LocalizedDisplayName(DisplayNameCz = "Nevyužitá rekreační", DisplayNameEn = "Unused recreational")]
    UnusedRecreational = 8
}