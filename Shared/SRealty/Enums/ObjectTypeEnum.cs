using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum ObjectTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Přízemní", DisplayNameEn = "Ground floor")]
    GroundFloor = 1,

    [SRealtyEnums(DisplayNameCz = "Patrový", DisplayNameEn = "Two-story")]
    TwoStory = 2
}