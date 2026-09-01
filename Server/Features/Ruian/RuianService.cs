using Microsoft.EntityFrameworkCore;
using Server.Features.Ruian.Entity;
using Server.Infrastructure.Database;

namespace Server.Features.Ruian;

/// <summary>Reads the RUIAN register; <see cref="RuianImporter"/> fills it on start-up.</summary>
public sealed class RuianService(AppDbContext appDbContext)
{
    /// <summary>Municipality with the given RUIAN code, or null when unknown.</summary>
    public async Task<RuianMunicipalityEntity?> FindMunicipalityByCodeAsync(int code,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.RuianMunicipalities
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Code == code, cancellationToken);
    }

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

    /// <summary>District with the given RUIAN code, or null when unknown.</summary>
    public async Task<RuianDistrictEntity?> FindDistrictByCodeAsync(int code,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.RuianDistricts
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Code == code, cancellationToken);
    }
}
