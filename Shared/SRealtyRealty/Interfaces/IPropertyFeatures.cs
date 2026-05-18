using Shared.SRealtyRealty.Enums;

namespace Shared.SRealtyRealty.Interfaces;

public interface IPropertyFeatures
{
    bool Balcony { get; set; }
    int? BalconyArea { get; set; }
    bool Basin { get; set; }
    int? BasinArea { get; set; }
    bool Cellar { get; set; }
    int? CellarArea { get; set; }
    bool Garage { get; set; }
    int? GarageArea { get; set; }
    int? GarageCount { get; set; }
    bool Loggia { get; set; }
    int? LoggiaArea { get; set; }
    bool ParkingLots { get; set; }
    int? ParkingCount { get; set; }
    bool Terrace { get; set; }
    int? TerraceArea { get; set; }
    int? GardenArea { get; set; }
    ElevatorEnum? Elevator { get; set; }
    FurnishingEnum? Furnished { get; set; }
}