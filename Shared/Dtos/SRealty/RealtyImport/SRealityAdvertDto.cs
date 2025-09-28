using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text.Json.Serialization;
using Shared.SRealtyRealty.Enums;
using Shared.SRealtyRealty.Interfaces;
using Shared.SRealtyRealty.ValueObjects;
using Shared.Validation.Attributes;

namespace Shared.Dtos.SRealty.RealtyImport;

[Serializable]
public sealed record SRealityAdvertDto : SRealtyAdvertCoreDto, IValidatableObject, ISRealtyProperty
{
    public double? Altitude { get; set; }
    public double? Latitude { get; set; }
    public int? RuianId { get; set; }
    public int? RuianLevel { get; set; }
    public int? UirId { get; set; }
    public int? UirLevel { get; set; }
    public string? CityPart { get; set; }
    public string? OrientationNumber { get; set; }
    public string? Street { get; set; }
    public string? HouseNumber { get; set; }

    [RequiredIfValue(nameof(AdvertType), [AdvertTypeEnum.Flat])]
    public bool? Balcony { get; set; }

    public int? BalconyArea { get; set; }

    [RequiredIfValue(nameof(AdvertType), [AdvertTypeEnum.House])]
    public bool? Basin { get; set; }

    public int? BasinArea { get; set; }

    [RequiredIfValue(nameof(AdvertType), [AdvertTypeEnum.Flat, AdvertTypeEnum.House])]
    public bool? Cellar { get; set; }

    public int? CellarArea { get; set; }
    public bool? Garage { get; set; }
    public int? GarageArea { get; set; }
    public int? GarageCount { get; set; }
    public bool? Loggia { get; set; }
    public int? LoggiaArea { get; set; }
    public bool? ParkingLots { get; set; }
    public int? ParkingCount { get; set; }
    public bool? Terrace { get; set; }
    public int? TerraceArea { get; set; }
    public int? GardenArea { get; set; }
    public ElevatorEnum? Elevator { get; set; }
    public FurnishingEnum? Furnished { get; set; }

    [RequiredIfValue(nameof(AdvertType),
        [AdvertTypeEnum.Flat, AdvertTypeEnum.House, AdvertTypeEnum.Commercial, AdvertTypeEnum.Other])]
    public BuildingConditionEnum? BuildingCondition { get; set; }

    [RequiredIfValue(nameof(AdvertType),
        [AdvertTypeEnum.Flat, AdvertTypeEnum.House, AdvertTypeEnum.Commercial, AdvertTypeEnum.Other])]
    public BuildingTypeEnum? BuildingType { get; set; }

    public ObjectTypeEnum? ObjectType { get; set; }
    public int? ApartmentNumber { get; set; }
    public int? EstateArea { get; set; }
    public int? FloorNumber { get; set; }
    public int? UsableArea { get; set; }
    public CircuitBreakerEnum CircuitBreaker { get; set; }

    [EnumValue(typeof(ElectricityTypeEnum))]
    [DefaultValue(null)]
    public List<ElectricityTypeEnum>? Electricity { get; set; }

    public PhaseCountEnum PhaseDistribution { get; set; }
    public bool? FtvPanels { get; set; }
    [DefaultValue(null)] public List<GasTypeEnum>? Gas { get; set; }
    [DefaultValue(null)] public List<SewerageTypeEnum>? Gully { get; set; }
    [DefaultValue(null)] public List<HeatingEnum>? Heating { get; set; }
    [DefaultValue(null)] public List<HeatingElementEnum>? HeatingElement { get; set; }
    [DefaultValue(null)] public List<HeatingSourceEnum>? HeatingSource { get; set; }
    public string? InternetConnectionProvider { get; set; }
    [DefaultValue(null)] public List<InternetConnectionTypeEnum>? InternetConnectionType { get; set; }
    public int? InternetConnectionSpeed { get; set; }
    [DefaultValue(null)] public List<TelecommunicationTypeEnum>? Telecommunication { get; set; }
    [DefaultValue(null)] public List<WaterTypeEnum>? Water { get; set; }
    [DefaultValue(null)] public List<WaterHeatingSourceEnum>? WaterHeatingSource { get; set; }
    [DefaultValue(null)] public List<WellTypeEnum>? WellType { get; set; }
    public EnergyRatingEnum? EnergyEfficiencyRating { get; set; }
    public EnergyPerformanceCertificateEnum? EnergyEfficiencyCertificate { get; set; }
    public double? EnergyPerformanceSummary { get; set; }

    public new IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResults = base.Validate(validationContext);
        foreach (var validationResult in validationResults)
        {
            yield return validationResult;
        }
        // yield return new ValidationResult("This is a test validation error", new[] { nameof(PropertyDescription) });
    }
};