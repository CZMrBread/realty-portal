namespace Server.Features.Ruian.Entity;

/// <summary>
/// A region (kraj, VÚSC) of the RUIAN address register. Keyed by the RUIAN code rather than a portal GUID,
/// because the code is the identifier the register itself uses and the one every advert refers to.
/// </summary>
public class RuianRegionEntity
{
    public int Code { get; set; }
    public required string Name { get; set; }

    public List<RuianDistrictEntity> Districts { get; set; } = [];
}
