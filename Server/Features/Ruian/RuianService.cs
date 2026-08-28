using Microsoft.EntityFrameworkCore;
using Server.Features.Ruian.Entity;
using Server.Infrastructure.Database;

namespace Server.Features.Ruian;

/// <summary>Reads the RUIAN register of regions, districts and municipalities. The register is read-only at runtime; <see cref="RuianImporter"/> fills it on start-up.</summary>
public sealed class RuianService(AppDbContext appDbContext)
{
    /// <summary>Municipality with the given RUIAN code, or null when the register does not know it.</summary>
    public async Task<RuianMunicipalityEntity?> FindMunicipalityByCodeAsync(int code,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.RuianMunicipalities
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Code == code, cancellationToken);
    }

    /// <summary>
    /// The one municipality called <paramref name="name"/>, compared without regard to case or accents, or null
    /// when there is none or when several share the name: guessing between two Lhotas would put the advert in
    /// the wrong district half the time, so no match is better than a wrong one.
    /// </summary>
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

    /// <summary>District with the given RUIAN code, or null when the register does not know it.</summary>
    public async Task<RuianDistrictEntity?> FindDistrictByCodeAsync(int code,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.RuianDistricts
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Code == code, cancellationToken);
    }
}
