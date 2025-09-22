using Shared.SRealtyRealty.ValueObjects;
using Shared.SRealtyRealty.Enums;

namespace Shared.SRealtyRealty.Interfaces;

public interface ISRealtyProperty : IPropertyCore, IPropertyDetails, IPropertyFeatures, IPropertyConstruction, IPropertyBusiness
{
    // PropertyAreas primitive properties
    int? UsableArea { get; }
    int? EstateArea { get; }
    int? BuildingArea { get; }
    int? BalconyArea { get; }
    int? CellarArea { get; }
    int? BasinArea { get; }
    int? LoggiaArea { get; }
    int? TerraceArea { get; }
    int? GardenArea { get; }
    int? FloorArea { get; }
    int? OfficesArea { get; }
    int? ProductionArea { get; }
    int? ShopArea { get; }
    int? StoreArea { get; }
    int? WorkshopArea { get; }
    int? NoLiveTotalArea { get; }
    double? CeilingHeight { get; }

    // UtilityConnections primitive properties
    ElectricityTypeEnum[]? Electricity { get; }
    GasTypeEnum[]? Gas { get; }
    WaterTypeEnum[]? Water { get; }
    SewerageTypeEnum[]? Sewerage { get; }
    HeatingTypeEnum[]? Heating { get; }
    HeatingElementEnum[]? HeatingElement { get; }
    HeatingSourceEnum[]? HeatingSource { get; }
    WaterHeatingSourceEnum[]? WaterHeatingSource { get; }
    TelecommunicationTypeEnum[]? Telecommunication { get; }
    InternetConnectionTypeEnum[]? InternetConnectionType { get; }
    string? InternetConnectionProvider { get; }
    int? InternetConnectionSpeed { get; }
    PhaseCountEnum? PhaseDistributions { get; }
    WellTypeEnum[]? WellType { get; }
    RoadTypeEnum[]? RoadType { get; }
    TransportTypeEnum[]? Transport { get; }

    CircuitBreakerEnum? CircuitBreaker { get; }
    SurroundingsTypeEnum? SurroundingsType { get; }

    string[]? Keywords { get; }
    string? Steps { get; }
    int? NumOwners { get; }

    string? MatterportUrl { get; }
    string? MapyPanoramaUrl { get; }
    int? Panorama { get; }

    // Computed ValueObject properties
    PropertyAreas Areas { get; }
    UtilityConnections? UtilityConnections { get; }
}