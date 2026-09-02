namespace Server.Features.Ruian.Entity;

/// <summary>A street (ulice) of the RUIAN address register, keyed by its RUIAN code.</summary>
public class RuianStreetEntity
{
    public int Code { get; set; }
    public required string Name { get; set; }

    /// <summary>The name lower-cased and stripped of accents, used to search the register.</summary>
    public required string SearchName { get; set; }

    public int MunicipalityCode { get; set; }
    public RuianMunicipalityEntity Municipality { get; set; } = null!;
}
