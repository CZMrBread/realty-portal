using Microsoft.EntityFrameworkCore;
using Server.Infrastructure.Database;
using Shared.SRealtyRealty.RealtyImport;

namespace Server.Features.SRealty;

public sealed class SRealtyService
{
    private readonly AppDbContext _context;

    public SRealtyService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SRealityAdvertDto> CreateAsync(SRealtyPropertyEntity entity,
        CancellationToken cancellationToken = default)
    {
        _context.SRealtyProperties.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }

    public async Task<SRealityAdvertDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var property = await _context.SRealtyProperties
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        return property?.ToDto();
    }

    public Task<SRealtyPropertyEntity?> GetByIdAsync(string advertRkId, string realtyAgentRkId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<SRealityAdvertDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var properties = await _context.SRealtyProperties
            .AsNoTracking()
            .Select(p => p.ToDto())
            .ToListAsync(cancellationToken);

        return properties;
    }

    public Task<SRealityAdvertDto?> UpdateAsync(Guid id, SRealtyPropertyEntity entity,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<SRealityAdvertDto> CreateFromDtoAsync(SRealityAdvertDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new SRealtyPropertyEntity
        {
            // Basic properties
            AdvertFunction = dto.AdvertFunction,
            AdvertLifetime = dto.AdvertLifetime,
            AdvertType = dto.AdvertType,
            AdvertSubtype = dto.AdvertSubtype,
            AdvertPrice = dto.AdvertPrice,
            AdvertPriceCurrency = dto.AdvertPriceCurrency,
            AdvertPriceUnit = dto.AdvertPriceUnit,
            City = dto.City,
            PropertyDescription = dto.PropertyDescription,
            InaccuracyLevel = dto.InaccuracyLevel,
            AdvertRkId = dto.AdvertRkId,
            RealtyAgentId = dto.RealtyAgentId,

            // Location properties
            Altitude = dto.Altitude,
            Latitude = dto.Latitude,
            RuianId = dto.RuianId,
            RuianLevel = dto.RuianLevel,
            UirId = dto.UirId,
            UirLevel = dto.UirLevel,
            CityPart = dto.CityPart,
            OrientationNumber = dto.OrientationNumber,
            Street = dto.Street,
            HouseNumber = dto.HouseNumber,

            // Boolean features
            Balcony = dto.Balcony,
            Basin = dto.Basin,
            Cellar = dto.Cellar,
            Garage = dto.Garage,
            Loggia = dto.Loggia,
            ParkingLots = dto.ParkingLots,
            Terrace = dto.Terrace,
            FtvPanels = dto.FtvPanels,

            // Area properties
            BalconyArea = dto.BalconyArea,
            BasinArea = dto.BasinArea,
            CellarArea = dto.CellarArea,
            GarageArea = dto.GarageArea,
            GarageCount = dto.GarageCount,
            LoggiaArea = dto.LoggiaArea,
            ParkingCount = dto.ParkingCount,
            TerraceArea = dto.TerraceArea,
            GardenArea = dto.GardenArea,

            // Building properties
            BuildingCondition = dto.BuildingCondition,
            BuildingType = dto.BuildingType,
            ObjectType = dto.ObjectType,
            Furnished = dto.Furnished,
            Elevator = dto.Elevator,
            ApartmentNumber = dto.ApartmentNumber,
            EstateArea = dto.EstateArea,
            FloorNumber = dto.FloorNumber,
            UsableArea = dto.UsableArea,

            // Utilities
            CircuitBreaker = dto.CircuitBreaker,
            Electricity = dto.Electricity,
            PhaseDistribution = dto.PhaseDistribution,
            Gas = dto.Gas,
            Gully = dto.Gully,
            Heating = dto.Heating,
            HeatingElement = dto.HeatingElement,
            HeatingSource = dto.HeatingSource,
            WaterHeatingSource = dto.WaterHeatingSource,
            WellType = dto.WellType,
            Water = dto.Water,

            // Internet & Communications
            InternetConnectionProvider = dto.InternetConnectionProvider,
            InternetConnectionSpeed = dto.InternetConnectionSpeed,
            InternetConnectionType = dto.InternetConnectionType,
            Telecommunication = dto.Telecommunication,

            // Energy efficiency
            EnergyEfficiencyRating = dto.EnergyEfficiencyRating,
            EnergyEfficiencyCertificate = dto.EnergyEfficiencyCertificate,
            EnergyPerformanceSummary = dto.EnergyPerformanceSummary
        };

        return await CreateAsync(entity, cancellationToken);
    }
}