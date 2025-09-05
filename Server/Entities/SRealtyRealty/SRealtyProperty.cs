using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Shared.SRealtyRealty.Enums;
using Server.Entities.SRealtyRealty.Interfaces;
using Server.Entities.SRealtyRealty.ValueObjects;

namespace Server.Entities.SRealtyRealty;

/// <summary>
/// Realty property entity representing a property listed on SReality platform
/// </summary>
[Table("SRealityProperties")]
[Index(nameof(RealtyAgencyId), nameof(CreatedAt))]
public sealed class SRealtyProperty : ISRealityProperty, ILocationData, IUtilityConnections, IAuctionProperty, ITimeStampedEntity
{
    private string? _keywordsJson;
    private List<string>? _keywordsCache;
    
    [Key]
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public Guid RealtyAgencyId { get; set; }
    
    [Required]
    public AdvertFunctionEnum AdvertFunction { get; set; }
    
    [Required]
    public AdvertLifetimeEnum AdvertLifetime { get; set; }
    
    [Required]
    public AdvertTypeEnum AdvertType { get; set; }
    
    [Required]
    public AdvertSubtypeEnum AdvertSubtype { get; set; }
    
    [Required]
    [StringLength(5000, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public double Price { get; set; }
    
    public AdvertPriceCurrencyEnum AdvertPriceCurrency { get; set; }
    public AdvertPriceUnitEnum AdvertPriceUnit { get; set; }
    
    public bool IsPriceNegotiable { get; set; }
    
    [StringLength(500)]
    public string? PriceNote { get; set; }
    
    [StringLength(500)]
    public string? PriceNoteEn { get; set; }
    
    [StringLength(500)]
    public string? PriceNoteRu { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Commission { get; set; }

    // Convenience property to work with price as value object
    [NotMapped]
    public PriceInformation PriceInfo => PriceInformation.Create(
        (decimal)Price, AdvertPriceCurrency, AdvertPriceUnit) with
    {
        IsNegotiable = IsPriceNegotiable,
        Note = PriceNote,
        NoteEn = PriceNoteEn,
        NoteRu = PriceNoteRu,
        Commission = Commission
    };
     
    
    [Required]
    [StringLength(255)]
    public string City { get; set; } = string.Empty;
    
    [StringLength(255)]
    public string? CityPart { get; set; }
    
    [StringLength(255)]
    public string? Street { get; set; }
    
    [StringLength(50)]
    public string? OrientationNumber { get; set; }
    
    [StringLength(50)]
    public string? DescriptiveNumber { get; set; }
    
    [Range(1, 3)]
    public int InaccuracyLevel { get; set; }
    
    public Point? Location { get; set; }
    
    public int? RuianCode { get; set; }
    public RuianLevelEnum? RuianLevel { get; set; }
    public int? UirCode { get; set; }
    public UirLevelEnum? UirLevel { get; set; }

    
    // Conditionally required based on property type
    public int? RoomCount { get; set; } // Required for Houses
    public int? ApartmentNumber { get; set; }
    public int? FloorNumber { get; set; } // Required for Flats

    // Facilities (nullable booleans for three-state logic)
    public bool? HasBalcony { get; set; } // Required for Flats
    public bool? HasBasin { get; set; } // Required for Houses  
    public bool? HasCellar { get; set; } // Required for Flats, Houses
    public bool? HasGarage { get; set; } // Required for Flats, Houses, Commercial
    public bool? HasLoggia { get; set; } // Required for Flats
    public bool? HasParkingSpaces { get; set; } // Required for Flats, Houses, Commercial
    public bool? HasTerrace { get; set; } // Required for Flats

    // Building characteristics
    public BuildingConditionEnum? BuildingCondition { get; set; }
    public BuildingTypeEnum? BuildingType { get; set; }
    public ObjectTypeEnum? ObjectType { get; set; }
    public OwnershipTypeEnum? OwnershipType { get; set; } // Required for Flats
    public CooperativeTransferEnum? CooperativeTransfer { get; set; } // Required when ownership=Cooperative
    
    #region Areas (Value Object Pattern)

    [Range(1, int.MaxValue)]
    public int? UsableArea { get; set; } // Required for Flats, Houses, Other, Commercial
    
    [Range(1, int.MaxValue)]
    public int? EstateArea { get; set; } // Required for Land, Houses
    
    public int? BuildingArea { get; set; }
    public int? BalconyArea { get; set; }
    public int? TerraceArea { get; set; }
    public int? CellarArea { get; set; }
    public int? GardenArea { get; set; }
    public int? LoggiaArea { get; set; }
    public int? FloorArea { get; set; }

    // Convenience property for working with areas as value object
    [NotMapped]
    public PropertyAreas Areas => new()
    {
        UsableArea = UsableArea,
        EstateArea = EstateArea,
        BuildingArea = BuildingArea,
        BalconyArea = BalconyArea,
        TerraceArea = TerraceArea,
        CellarArea = CellarArea,
        GardenArea = GardenArea,
        LoggiaArea = LoggiaArea
    };

    #endregion

    #region IUtilityConnections Implementation

    public ElectricityTypeEnum? ElectricityType { get; set; }
    public GasTypeEnum? GasType { get; set; }
    public WaterTypeEnum? WaterType { get; set; }
    public SewerageTypeEnum? SewerageType { get; set; }
    public HeatingTypeEnum? HeatingType { get; set; }
    public TelecommunicationTypeEnum? TelecommunicationType { get; set; }

    // Extended utility information
    public HeatingElementEnum? HeatingElement { get; set; }
    public HeatingSourceEnum? HeatingSource { get; set; }
    public WaterHeatingSourceEnum? WaterHeatingSource { get; set; }
    public InternetConnectionTypeEnum? InternetConnectionType { get; set; }
    
    [StringLength(255)]
    public string? InternetProvider { get; set; }
    
    [Range(1, 10000)]
    public int? InternetSpeedMbps { get; set; }

    #endregion

    #region Energy Efficiency (Value Object Pattern)

    public EnergyRatingEnum? EnergyRating { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    [Range(0, 9999)]
    public decimal? EnergyPerformanceSummary { get; set; }
    
    public CertificateTypeEnum? EnergyCertificateType { get; set; }
    public byte[]? EnergyCertificateDocument { get; set; }
    public bool IsLowEnergyBuilding { get; set; }

    // Convenience property for energy efficiency value object
    [NotMapped]
    public EnergyEfficiency EnergyInfo => new()
    {
        Rating = EnergyRating,
        PerformanceSummary = EnergyPerformanceSummary,
        CertificateType = EnergyCertificateType,
        CertificateDocument = EnergyCertificateDocument,
        IsLowEnergy = IsLowEnergyBuilding
    };

    #endregion

    #region IAuctionProperty Implementation

    public DateTime? AuctionDate { get; set; }
    public DateTime? AuctionTour1Date { get; set; }
    public DateTime? AuctionTour2Date { get; set; }
    public AuctionKindEnum? AuctionKind { get; set; }
    
    [StringLength(500)]
    public string? AuctionPlace { get; set; }
    
    public BiddingTypeEnum? BiddingType { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? AuctionSecurityDeposit { get; set; } // Required for Auctions
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? ExpertValuation { get; set; } // Required for Auctions
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? MinimumBid { get; set; } // Required for English auctions
    
    public byte[]? AuctionAnnouncementPdf { get; set; }
    public byte[]? ExpertReportPdf { get; set; }

    #endregion

    #region Additional Property Details

    // Dates
    public DateTime? ConstructionYear { get; set; }
    public DateTime? ReconstructionYear { get; set; }
    public DateTime? MoveInDate { get; set; } // Required for Rent
    public DateTime? FirstViewingDate { get; set; }
    public DateTime? FirstViewingDateTo { get; set; }
    public DateTime? SaleStartDate { get; set; }

    // Technical specifications
    [Column(TypeName = "decimal(5,2)")]
    [Range(1.8, 10)]
    public decimal? CeilingHeightMeters { get; set; }
    
    public CircuitBreakerEnum? CircuitBreaker { get; set; }
    public PhaseCountEnum? ElectricalPhases { get; set; }
    
    // Property features
    public AccessibilityEnum? Accessibility { get; set; }
    public ElevatorEnum? ElevatorAccess { get; set; }
    public FurnishingEnum? FurnishingLevel { get; set; }
    public FlatTypeEnum? FlatType { get; set; }
    
    // Counts and additional areas
    public int? FloorsCount { get; set; }
    public int? GarageCount { get; set; }
    public int? ParkingSpacesCount { get; set; }
    public int? UndergroundFloorsCount { get; set; }
    
    // Financial details
    [Column(TypeName = "decimal(18,2)")]
    public decimal? MonthlyFeesAmount { get; set; }
    
    [Column(TypeName = "decimal(5,2)")]
    [Range(0, 100)]
    public decimal? MortgagePercent { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? RefundableDeposit { get; set; }

    // Share ownership (for Shares type)
    public int? ShareNumerator { get; set; } // Required for Shares
    public int? ShareDenominator { get; set; } // Required for Shares
    public int? CommonAreaShareNumerator { get; set; }
    public int? CommonAreaShareDenominator { get; set; }

    // Special features
    public bool HasPhotovoltaics { get; set; }
    public bool HasSolarPanels { get; set; }
    public bool HasSwimmingPool { get; set; }
    public bool IsExclusiveListing { get; set; }
    public bool TenantPaysNoCommission { get; set; }

    #endregion

    #region Multilingual Content

    [StringLength(5000)]
    public string? DescriptionEnglish { get; set; }
    
    [StringLength(5000)]
    public string? DescriptionRussian { get; set; }

    #endregion

    #region External References

    [StringLength(1000)]
    [Url]
    public string? MapsPanoramaUrl { get; set; }
    
    [StringLength(1000)]
    [Url]
    public string? VirtualTourUrl { get; set; }

    // Seller reference
    public Guid? SellerId { get; set; }
    
    [StringLength(255)]
    public string? SellerExternalId { get; set; }

    #endregion

    #region Keywords (JSON Storage)

    [StringLength(2000)]
    [Column("KeywordsJson")]
    public string? KeywordsJson
    {
        get => _keywordsJson;
        set
        {
            _keywordsJson = value;
            _keywordsCache = null; // Invalidate cache
        }
    }

    public IReadOnlyCollection<string> GetKeywords()
    {
        if (_keywordsCache is not null)
            return _keywordsCache.AsReadOnly();

        if (string.IsNullOrWhiteSpace(KeywordsJson))
        {
            _keywordsCache = new List<string>();
            return _keywordsCache.AsReadOnly();
        }

        try
        {
            _keywordsCache = JsonSerializer.Deserialize<List<string>>(KeywordsJson) ?? new List<string>();
        }
        catch (JsonException)
        {
            _keywordsCache = new List<string>();
        }

        return _keywordsCache.AsReadOnly();
    }

    public void SetKeywords(IEnumerable<string> keywords)
    {
        var keywordList = keywords?.Where(k => !string.IsNullOrWhiteSpace(k))
                                 .Select(k => k.Trim())
                                 .Distinct()
                                 .ToList() ?? new List<string>();

        _keywordsCache = keywordList;
        KeywordsJson = keywordList.Count > 0 ? JsonSerializer.Serialize(keywordList) : null;
    }

    #endregion
    

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public bool IsActive { get; set; } = true;

    #region Entity Framework Configuration Helper
    

    #endregion
}