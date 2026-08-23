using Shared.SRealty.Advert.Enums;

namespace Server.Features.SRealty.Advert.Entity;

public partial class SrealityAdvertEntity
{
    public bool? Balcony { get; set; }
    public int? BalconyArea { get; set; }
    public bool? Loggia { get; set; }
    public int? LoggiaArea { get; set; }
    public bool? Terrace { get; set; }
    public int? TerraceArea { get; set; }
    public bool? Cellar { get; set; }
    public int? CellarArea { get; set; }
    public bool? Basin { get; set; }
    public int? BasinArea { get; set; }
    public bool? Garage { get; set; }
    public int? GarageCount { get; set; }
    public bool? ParkingLots { get; set; }
    public int? Parking { get; set; }
    public FurnishingEnum? Furnished { get; set; }
    public ElevatorEnum? Elevator { get; set; }
    public bool? FtvPanels { get; set; }
    public bool? SolarPanels { get; set; }
}
