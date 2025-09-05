namespace Server.Entities.SRealtyRealty.ValueObjects;

/// <summary>
/// Encapsulates area measurements for different property sections
/// </summary>
public record PropertyAreas
{
    public int? UsableArea { get; init; }
    public int? EstateArea { get; init; }
    public int? BuildingArea { get; init; }
    public int? BalconyArea { get; init; }
    public int? TerraceArea { get; init; }
    public int? CellarArea { get; init; }
    public int? GardenArea { get; init; }
    public int? LoggiaArea { get; init; }
    
    public static PropertyAreas Empty => new();
}