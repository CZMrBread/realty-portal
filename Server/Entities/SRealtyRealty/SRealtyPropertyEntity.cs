using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.SRealty.RealtyImport;
using Shared.SRealtyRealty.Enums;
using Shared.SRealtyRealty.Interfaces;
using Shared.SRealtyRealty.ValueObjects;

namespace Server.Entities.SRealtyRealty;

[Table("SRealityProperties")]
public sealed class SRealtyPropertyEntity : ITimeStampedEntity, ISRealtyProperty
{
    public SRealtyPropertyEntity()
    {
        Id = Guid.CreateVersion7();
    }
    
    [Key]
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public AdvertFunctionEnum AdvertFunction { get; set; }
    public AdvertLifetimeEnum AdvertLifetime { get; set; }
    public double AdvertPrice { get; set; }
    public AdvertPriceCurrencyEnum AdvertPriceCurrency { get; set; }
    public AdvertPriceUnitEnum AdvertPriceUnit { get; set; }
    public AdvertTypeEnum AdvertType { get; set; }
    public string PropertyDescription { get; set; }
    public string City { get; set; }
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
    public int InaccuracyLevel { get; set; }
    public string? AdvertRkId { get; set; }
    public string? RealtyAgentId { get; set; }
    public Guid? RealtyAgentRkId { get; set; }
    public AdvertSubtypeEnum AdvertSubtype { get; set; }
    public bool Balcony { get; set; }
    public int? BalconyArea { get; set; }
    public bool Basin { get; set; }
    public int? BasinArea { get; set; }
    public bool Cellar { get; set; }
    public int? CellarArea { get; set; }
    public bool Garage { get; set; }
    public int? GarageArea { get; set; }
    public int? GarageCount { get; set; }
    public bool Loggia { get; set; }
    public int? LoggiaArea { get; set; }
    public bool ParkingLots { get; set; }
    public int? ParkingCount { get; set; }
    public bool Terrace { get; set; }
    public int? TerraceArea { get; set; }
    public int? GardenArea { get; set; }
    public ElevatorEnum? Elevator { get; set; }
    public FurnishingEnum? Furnished { get; set; }
    public BuildingConditionEnum? BuildingCondition { get; set; }
    public BuildingTypeEnum? BuildingType { get; set; }
    public ObjectTypeEnum? ObjectType { get; set; }
    public int? ApartmentNumber { get; set; }
    public int? EstateArea { get; set; }
    public int? FloorNumber { get; set; }
    public int? UsableArea { get; set; }
    public CircuitBreakerEnum? CircuitBreaker { get; set; }
    public List<ElectricityTypeEnum>? Electricity { get; set; }
    public PhaseCountEnum? PhaseDistribution { get; set; }
    public bool FtvPanels { get; set; }
    public List<GasTypeEnum>? Gas { get; set; }
    public List<SewerageTypeEnum>? Gully { get; set; }
    public List<HeatingEnum>? Heating { get; set; }
    public List<HeatingElementEnum>? HeatingElement { get; set; }
    public List<HeatingSourceEnum>? HeatingSource { get; set; }
    public string? InternetConnectionProvider { get; set; }
    public List<InternetConnectionTypeEnum>? InternetConnectionType { get; set; }
    public int? InternetConnectionSpeed { get; set; }
    public List<TelecommunicationTypeEnum>? Telecommunication { get; set; }
    public List<WaterTypeEnum>? Water { get; set; }
    public List<WaterHeatingSourceEnum>? WaterHeatingSource { get; set; }
    public List<WellTypeEnum>? WellType { get; set; }
    public EnergyRatingEnum? EnergyEfficiencyRating { get; set; }
    public EnergyPerformanceCertificateEnum? EnergyEfficiencyCertificate { get; set; }
    public double? EnergyPerformanceSummary { get; set; }

    public SRealityAdvertDto ToDto()
    {
        return new SRealityAdvertDto
        {
            AdvertFunction = AdvertFunction,
            AdvertLifetime = AdvertLifetime,
            AdvertPrice = AdvertPrice,
            AdvertPriceCurrency = AdvertPriceCurrency,
            AdvertPriceUnit = AdvertPriceUnit,
            AdvertType = AdvertType,
            PropertyDescription = PropertyDescription,
            City = City,
            Altitude = Altitude,
            Latitude = Latitude,
            RuianId = RuianId,
            RuianLevel = RuianLevel,
            UirId = UirId,
            UirLevel = UirLevel,
            CityPart = CityPart,
            OrientationNumber = OrientationNumber,
            Street = Street,
            HouseNumber = HouseNumber,
            InaccuracyLevel = InaccuracyLevel,
            AdvertRkId = AdvertRkId,
            RealtyAgentId = RealtyAgentId,
            RealtyAgentRkId = RealtyAgentRkId,
            AdvertSubtype = AdvertSubtype,

            // Boolean features
            Balcony = Balcony,
            Basin = Basin,
            Cellar = Cellar,
            Garage = Garage,
            Loggia = Loggia,
            ParkingLots = ParkingLots,
            Terrace = Terrace,
            FtvPanels = FtvPanels,

            // Area properties
            BalconyArea = BalconyArea,
            BasinArea = BasinArea,
            CellarArea = CellarArea,
            GarageArea = GarageArea,
            GarageCount = GarageCount,
            LoggiaArea = LoggiaArea,
            ParkingCount = ParkingCount,
            TerraceArea = TerraceArea,
            GardenArea = GardenArea,

            // Building properties
            BuildingCondition = BuildingCondition,
            BuildingType = BuildingType,
            ObjectType = ObjectType,
            Furnished = Furnished,
            Elevator = Elevator,
            ApartmentNumber = ApartmentNumber,
            EstateArea = EstateArea,
            FloorNumber = FloorNumber,
            UsableArea = UsableArea,

            // Utilities
            CircuitBreaker = CircuitBreaker,
            Electricity = Electricity,
            PhaseDistribution = PhaseDistribution,
            Gas = Gas,
            Gully = Gully,
            Heating = Heating,
            HeatingElement = HeatingElement,
            HeatingSource = HeatingSource,
            WaterHeatingSource = WaterHeatingSource,
            WellType = WellType,
            Water = Water,

            // Internet & Communications
            InternetConnectionProvider = InternetConnectionProvider,
            InternetConnectionSpeed = InternetConnectionSpeed,
            InternetConnectionType = InternetConnectionType
        };
    }
}