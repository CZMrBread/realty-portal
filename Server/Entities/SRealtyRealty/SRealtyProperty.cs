using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Shared.SRealtyRealty.Enums;
using Shared.SRealtyRealty.Interfaces;
using Shared.SRealtyRealty.ValueObjects;
using Server.Entities.SRealtyRealty.Interfaces;
namespace Server.Entities.SRealtyRealty;
/// <summary>
/// Realty property entity representing a property listed on SRealty platform
/// </summary>
[Table("SRealityProperties")]
[Index(nameof(UpdatedAt))]
[Index(nameof(AdvertType))]
[Index(nameof(AdvertSubtype))]
public sealed class SRealtyProperty : ITimeStampedEntity, ISRealtyProperty
{
    SRealtyProperty(string advertRkId)
    {
        Id = Guid.CreateVersion7();
        AdvertRkId = advertRkId;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    
    // Base Entity Properties
    [Key]
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public string AdvertRkId { get; init; }

    [Required]
    public AdvertFunctionEnum AdvertFunction { get; set; }

    [Required]
    public AdvertLifetimeEnum AdvertLifetime { get; set; }

    [Required]
    public Price Price { get; set; } = null!;

    [Required]
    public AdvertTypeEnum AdvertType { get; set; }

    [Required]
    public PropertyDescription Description { get; set; } = null!;

    [Required]
    public Location Location { get; set; } = null!;

    // Property Details
    [Required]
    public AdvertSubtypeEnum AdvertSubtype { get; set; }
    public AdvertRoomCountEnum? AdvertRoomCount { get; set; }
    public int? ApartmentNumber { get; set; }
    public int? FloorNumber { get; set; }
    public int? Floors { get; set; }
    public int? UndergroundFloors { get; set; }
    public FlatClassEnum? FlatClass { get; set; }
    public ObjectKindEnum? ObjectKind { get; set; }
    public ObjectLocationEnum? ObjectLocation { get; set; }

    // Property Features & Amenities
    public bool? HasBalcony { get; set; }
    public bool? HasLoggia { get; set; }
    public bool? HasTerrace { get; set; }
    public bool? HasCellar { get; set; }
    public bool? HasGarage { get; set; }
    public bool? HasParkingLots { get; set; }
    public bool? HasBasin { get; set; }
    public bool? HasGarret { get; set; }
    public int? GarageCount { get; set; }
    public int? Parking { get; set; }
    public AccessibilityEnum? EasyAccess { get; set; }
    public ElevatorEnum? HasElevator { get; set; }
    public FurnishingEnum? Furnished { get; set; }

    // Construction & Building Properties
    public BuildingConditionEnum? BuildingCondition { get; set; }
    public BuildingTypeEnum? BuildingType { get; set; }
    public ObjectTypeEnum? ObjectType { get; set; }
    public int? AcceptanceYear { get; set; }
    public int? ObjectAge { get; set; }
    public int? ReconstructionYear { get; set; }
    [Column(TypeName = "date")]
    public DateTime? BeginningDate { get; set; }
    [Column(TypeName = "date")]
    public DateTime? FinishDate { get; set; }

    // Energy & Environmental
    public bool? IsLowEnergy { get; set; }
    public EnergyRatingEnum? EnergyEfficiencyRating { get; set; }
    public CertificateTypeEnum? EnergyPerformanceCertificate { get; set; }
    public double? EnergyPerformanceSummary { get; set; }
    public bool? SolarPanels { get; set; }
    public bool? FtvPanels { get; set; }

    // Business & Transaction Properties
    public OwnershipTypeEnum? Ownership { get; set; }
    public float? PersonalTransferAmount { get; set; }
    public LeaseTypeEnum? LeaseType { get; set; }
    [Column(TypeName = "date")]
    public DateOnly? ReadyDate { get; set; }
    [Column(TypeName = "date")]
    public DateOnly? SaleDate { get; set; }
    [MaxLength(50)]
    public string? AdvertCode { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public double? Commission { get; set; }
    [MaxLength(500)]
    public string? CostOfLiving { get; set; }
    public bool? ExclusivelyAtRk { get; set; }
    public bool? TenantNotPayCommission { get; set; }
    public bool? Mortgage { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public double? MortgagePercent { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public double? RefundableDeposit { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public double? SporPercent { get; set; }
    public int? Annuity { get; set; }

    // Complex Value Objects
    public PropertyAreas Areas { get; set; } = new();

    public AuctionDetails? AuctionDetails { get; set; }

    public ShareDetails? ShareDetails { get; set; }

    public UtilityConnections? UtilityConnections { get; set; }

    // Technical & Infrastructure
    public CircuitBreakerEnum? CircuitBreaker { get; set; }
    public SurroundingsTypeEnum? SurroundingsType { get; set; }

    // Agency & Project Relations
    public int? ProjectId { get; set; }

    [MaxLength(100)]
    public string? ProjectRkId { get; set; }

    public int? SellerId { get; set; }

    [MaxLength(100)]
    public string? SellerRkId { get; set; }

    // Additional Information
    [MaxLength(1000)]
    [Column(TypeName = "text[]")]
    public string[]? Keywords { get; set; }

    [MaxLength(200)]
    public string? Steps { get; set; }

    public int? NumOwners { get; set; }

    // Media & Virtual Tours
    [MaxLength(500)]
    public string? MatterportUrl { get; set; }

    [MaxLength(500)]
    public string? MapyPanoramaUrl { get; set; }

    public int? Panorama { get; set; }

    // Additional Optional Fields from Schema
    public ProtectionEnum? Protection { get; set; }

    public ExtraInfoEnum? ExtraInfo { get; set; }

    public DateTime? FirstTourDate { get; set; }

    public DateTime? FirstTourDateTo { get; set; }

    public double? PriceAuctionPrincipal { get; set; }

    [Column(TypeName = "bytea")]
    public byte[]? EnergyPerformanceAttachment { get; set; }
}