using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Number of above-ground storeys the building has.</summary>
public enum ObjectTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Přízemní", DisplayNameEn = "Ground floor")]
    GroundFloor = 1,

    [LocalizedDisplayName(DisplayNameCz = "Patrový", DisplayNameEn = "Two-story")]
    TwoStory = 2
}