using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>How the building sits among its neighbours, for example detached or in a terrace.</summary>
public enum ObjectKindEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Řadový", DisplayNameEn = "Row house")]
    RowHouse = 1,

    [LocalizedDisplayName(DisplayNameCz = "Rohový", DisplayNameEn = "Corner house")]
    CornerHouse = 2,

    [LocalizedDisplayName(DisplayNameCz = "V bloku", DisplayNameEn = "In block")]
    InBlock = 3,

    [LocalizedDisplayName(DisplayNameCz = "Samostatně stojící", DisplayNameEn = "Detached")]
    Detached = 4
}