namespace Server.Features.Ruian.Entity;

/// <summary>A district (okres) of the RUIAN address register, keyed by its RUIAN code.</summary>
public class RuianDistrictEntity
{
    public int Code { get; set; }
    public required string Name { get; set; }

    public int RegionCode { get; set; }
    public RuianRegionEntity Region { get; set; } = null!;

    public List<RuianMunicipalityEntity> Municipalities { get; set; } = [];
}
