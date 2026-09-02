using Microsoft.EntityFrameworkCore;
using Server.Features.Ruian.Entity;
using Server.Infrastructure.Database;
using Shared.Ruian;

namespace Server.Features.Ruian;

/// <summary>Reads the RUIAN register; <see cref="RuianImporter"/> fills it on start-up.</summary>
public sealed class RuianService(AppDbContext appDbContext)
{
    // --- by code ---

    /// <summary>District with the given RUIAN code, or null when unknown.</summary>
    public async Task<RuianDistrictEntity?> FindDistrictByCodeAsync(int code,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.RuianDistricts
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Code == code, cancellationToken);
    }

    /// <summary>Municipality with the given RUIAN code, or null when unknown.</summary>
    public async Task<RuianMunicipalityEntity?> FindMunicipalityByCodeAsync(int code,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.RuianMunicipalities
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Code == code, cancellationToken);
    }

    /// <summary>Street with the given RUIAN code and its municipality loaded, or null when unknown.</summary>
    public async Task<RuianStreetEntity?> FindStreetByCodeAsync(int code,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.RuianStreets
            .AsNoTracking()
            .Include(s => s.Municipality)
            .FirstOrDefaultAsync(s => s.Code == code, cancellationToken);
    }

    /// <summary>Address point with the given RUIAN code and its municipality, part and street loaded, or null.</summary>
    public async Task<RuianAddressPointEntity?> FindAddressPointByCodeAsync(int code,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.RuianAddressPoints
            .AsNoTracking()
            .Include(a => a.Municipality)
            .Include(a => a.MunicipalityPart)
            .Include(a => a.Street)
            .FirstOrDefaultAsync(a => a.Code == code, cancellationToken);
    }

    // --- by name ---

    /// <summary>The one municipality of that name, ignoring case and accents; null when none or many match.</summary>
    public async Task<RuianMunicipalityEntity?> FindMunicipalityByNameAsync(string name,
        CancellationToken cancellationToken = default)
    {
        var searchName = RuianNames.Normalize(name);
        if (searchName.Length == 0)
        {
            return null;
        }

        var candidates = await appDbContext.RuianMunicipalities
            .AsNoTracking()
            .Where(m => m.SearchName == searchName)
            .Take(2)
            .ToListAsync(cancellationToken);
        return candidates.Count == 1 ? candidates[0] : null;
    }

    /// <summary>The one street of that name in the municipality, ignoring case and accents; null when none or many.</summary>
    public async Task<RuianStreetEntity?> FindStreetByNameAsync(int municipalityCode, string name,
        CancellationToken cancellationToken = default)
    {
        var searchName = RuianNames.Normalize(name);
        if (searchName.Length == 0)
        {
            return null;
        }

        var candidates = await appDbContext.RuianStreets
            .AsNoTracking()
            .Where(s => s.MunicipalityCode == municipalityCode && s.SearchName == searchName)
            .Take(2)
            .ToListAsync(cancellationToken);
        return candidates.Count == 1 ? candidates[0] : null;
    }

    /// <summary>
    /// The one address point in the municipality with the given descriptive number, narrowed by the street and
    /// part names and the orientation number when given; null when none or many match.
    /// </summary>
    public async Task<RuianAddressPointEntity?> FindAddressPointAsync(int municipalityCode, int houseNumber,
        string? streetName, string? partName, int? orientationNumber, string? orientationLetter,
        CancellationToken cancellationToken = default)
    {
        var query = appDbContext.RuianAddressPoints
            .AsNoTracking()
            .Include(a => a.Municipality)
            .Include(a => a.MunicipalityPart)
            .Include(a => a.Street)
            .Where(a => a.MunicipalityCode == municipalityCode
                        && a.HouseNumberType == HouseNumberTypeEnum.Descriptive
                        && a.HouseNumber == houseNumber);

        if (!string.IsNullOrWhiteSpace(streetName))
        {
            var street = RuianNames.Normalize(streetName);
            query = query.Where(a => a.Street != null && a.Street.SearchName == street);
        }

        if (!string.IsNullOrWhiteSpace(partName))
        {
            var part = RuianNames.Normalize(partName);
            query = query.Where(a => a.MunicipalityPart.SearchName == part);
        }

        if (orientationNumber is { } number)
        {
            query = query.Where(a => a.OrientationNumber == number);
            if (!string.IsNullOrEmpty(orientationLetter))
            {
                var letter = orientationLetter.ToLowerInvariant();
                query = query.Where(a => a.OrientationLetter != null && a.OrientationLetter.ToLower() == letter);
            }
        }

        var candidates = await query.Take(2).ToListAsync(cancellationToken);
        return candidates.Count == 1 ? candidates[0] : null;
    }

    // --- lists for the address picker ---

    /// <summary>All regions, by name.</summary>
    public async Task<List<RuianRegionEntity>> GetRegionsAsync(CancellationToken cancellationToken = default)
    {
        return await appDbContext.RuianRegions
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>Districts of the region, by name.</summary>
    public async Task<List<RuianDistrictEntity>> GetDistrictsAsync(int regionCode,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.RuianDistricts
            .AsNoTracking()
            .Where(d => d.RegionCode == regionCode)
            .OrderBy(d => d.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>Municipalities of the district, by name.</summary>
    public async Task<List<RuianMunicipalityEntity>> GetMunicipalitiesAsync(int districtCode,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.RuianMunicipalities
            .AsNoTracking()
            .Where(m => m.DistrictCode == districtCode)
            .OrderBy(m => m.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>Parts of the municipality, by name.</summary>
    public async Task<List<RuianMunicipalityPartEntity>> GetMunicipalityPartsAsync(int municipalityCode,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.RuianMunicipalityParts
            .AsNoTracking()
            .Where(p => p.MunicipalityCode == municipalityCode)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>Streets of the municipality, by name; empty where it has none.</summary>
    public async Task<List<RuianStreetEntity>> GetStreetsAsync(int municipalityCode,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.RuianStreets
            .AsNoTracking()
            .Where(s => s.MunicipalityCode == municipalityCode)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>Address points of the street, or the street-less ones of the part; by house number.</summary>
    public async Task<List<RuianAddressPointEntity>> GetAddressPointsAsync(int? streetCode, int? partCode,
        CancellationToken cancellationToken = default)
    {
        var query = appDbContext.RuianAddressPoints.AsNoTracking();
        query = streetCode is { } street
            ? query.Where(a => a.StreetCode == street)
            : query.Where(a => a.MunicipalityPartCode == partCode && a.StreetCode == null);

        return await query
            .OrderBy(a => a.HouseNumberType)
            .ThenBy(a => a.HouseNumber)
            .ThenBy(a => a.OrientationNumber)
            .ToListAsync(cancellationToken);
    }
}
