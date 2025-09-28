namespace Shared.SRealtyRealty.Enums;

public enum ObjectTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Přízemní", DisplayNameEn = "Ground floor")]
    GroundFloor = 1,

    [SRealtyEnums(DisplayNameCz = "Patrový", DisplayNameEn = "Two-story")]
    TwoStory = 2
}