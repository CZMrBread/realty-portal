using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum ObjectKindEnum
{
    [SRealtyEnums(DisplayNameCz = "Řadový", DisplayNameEn = "Row house")]
    RowHouse = 1,

    [SRealtyEnums(DisplayNameCz = "Rohový", DisplayNameEn = "Corner house")]
    CornerHouse = 2,

    [SRealtyEnums(DisplayNameCz = "V bloku", DisplayNameEn = "In block")]
    InBlock = 3,

    [SRealtyEnums(DisplayNameCz = "Samostatně stojící", DisplayNameEn = "Detached")]
    Detached = 4
}