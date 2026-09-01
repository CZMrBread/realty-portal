namespace Server.Features.Ruian.Entity;

/// <summary>A region (kraj, VÚSC) of the RUIAN address register, keyed by its RUIAN code.</summary>
public class RuianRegionEntity
{
    public int Code { get; set; }
    public required string Name { get; set; }

    public List<RuianDistrictEntity> Districts { get; set; } = [];
}
