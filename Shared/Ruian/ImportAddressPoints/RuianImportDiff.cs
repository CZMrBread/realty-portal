namespace Shared.Ruian.ImportAddressPoints;

/// <summary>How many rows of one RUIAN table an import inserted or overwrote, and how many it deleted.</summary>
public sealed record RuianImportDiff
{
    public int Upserted { get; set; }

    public int Deleted { get; set; }
}
