namespace Server.Features.Ruian.Entity;

/// <summary>A municipality (obec) of the RUIAN address register, keyed by its RUIAN code.</summary>
public class RuianMunicipalityEntity
{
    public int Code { get; set; }
    public required string Name { get; set; }

    /// <summary>The name lower-cased and stripped of accents, used to match adverts to municipalities.</summary>
    public required string SearchName { get; set; }

    public int DistrictCode { get; set; }
    public RuianDistrictEntity District { get; set; } = null!;
}
