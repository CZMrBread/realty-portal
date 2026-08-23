using Shared.SRealty.Advert.Enums;

namespace Server.Features.SRealty.Advert.Entity;

public partial class SrealityAdvertEntity
{
    public BuildingConditionEnum? BuildingCondition { get; set; }
    public BuildingTypeEnum? BuildingType { get; set; }
    public ObjectTypeEnum? ObjectType { get; set; }
    public ObjectKindEnum? ObjectKind { get; set; }
    public FlatClassEnum? FlatClass { get; set; }
    public int? FloorNumber { get; set; }
    public int? Floors { get; set; }
    public int? UndergroundFloors { get; set; }
    public int? ApartmentNumber { get; set; }
    public bool? Garret { get; set; }
    public AccessibilityEnum? EasyAccess { get; set; }
    public int? AcceptanceYear { get; set; }
    public int? ObjectAge { get; set; }
    public int? ReconstructionYear { get; set; }
    public DateOnly? BeginningDate { get; set; }
    public DateOnly? FinishDate { get; set; }
    public string? Steps { get; set; }
}
