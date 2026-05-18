using Shared.SRealtyRealty.Enums;

namespace Shared.SRealtyRealty.Interfaces;

public interface IPropertyConstruction
{
    BuildingConditionEnum? BuildingCondition { get; set; }
    BuildingTypeEnum? BuildingType { get; set; }
    ObjectTypeEnum? ObjectType { get; set; }
    int? ApartmentNumber { get; set; }
    int? EstateArea { get; set; }
    int? FloorNumber { get; set; }
    int? UsableArea { get; set; }
}