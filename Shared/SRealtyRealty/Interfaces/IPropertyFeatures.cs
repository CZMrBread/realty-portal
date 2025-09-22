using Shared.SRealtyRealty.Enums;

namespace Shared.SRealtyRealty.Interfaces;

public interface IPropertyFeatures
{
    bool? HasBalcony { get; }
    bool? HasLoggia { get; }
    bool? HasTerrace { get; }
    bool? HasCellar { get; }
    bool? HasGarage { get; }
    bool? HasParkingLots { get; }
    bool? HasBasin { get; }
    bool? HasGarret { get; }

    int? GarageCount { get; }
    int? Parking { get; }

    AccessibilityEnum? EasyAccess { get; }
    ElevatorEnum? HasElevator { get; }
    FurnishingEnum? Furnished { get; }
}