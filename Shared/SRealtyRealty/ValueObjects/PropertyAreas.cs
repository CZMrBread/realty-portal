namespace Shared.SRealtyRealty.ValueObjects;

public record PropertyAreas
{
    public int? UsableArea { get; init; }

    public int? EstateArea { get; init; }

    public int? BuildingArea { get; init; }

    public int? BalconyArea { get; init; }

    public int? CellarArea { get; init; }

    public int? BasinArea { get; init; }

    public int? LoggiaArea { get; init; }

    public int? TerraceArea { get; init; }

    public int? GardenArea { get; init; }

    public int? FloorArea { get; init; }

    public int? OfficesArea { get; init; }

    public int? ProductionArea { get; init; }

    public int? ShopArea { get; init; }

    public int? StoreArea { get; init; }

    public int? WorkshopArea { get; init; }

    public int? NoLiveTotalArea { get; init; }

    public double? CeilingHeight { get; init; }
}