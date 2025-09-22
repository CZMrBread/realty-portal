using Shared.SRealtyRealty.Enums;

namespace Shared.SRealtyRealty.ValueObjects;

public record UtilityConnections
{
    public ElectricityTypeEnum[]? Electricity { get; init; }

    public GasTypeEnum[]? Gas { get; init; }

    public WaterTypeEnum[]? Water { get; init; }

    public SewerageTypeEnum[]? Sewerage { get; init; }

    public HeatingTypeEnum[]? Heating { get; init; }

    public HeatingElementEnum[]? HeatingElement { get; init; }

    public HeatingSourceEnum[]? HeatingSource { get; init; }

    public WaterHeatingSourceEnum[]? WaterHeatingSource { get; init; }

    public TelecommunicationTypeEnum[]? Telecommunication { get; init; }

    public InternetConnectionTypeEnum[]? InternetConnectionType { get; init; }

    public string? InternetConnectionProvider { get; init; }

    public int? InternetConnectionSpeed { get; init; }

    public PhaseCountEnum? PhaseDistributions { get; init; }

    public WellTypeEnum[]? WellType { get; init; }

    public RoadTypeEnum[]? RoadType { get; init; }

    public TransportTypeEnum[]? Transport { get; init; }
}