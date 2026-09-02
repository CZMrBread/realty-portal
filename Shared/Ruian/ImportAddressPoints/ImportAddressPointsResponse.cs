namespace Shared.Ruian.ImportAddressPoints;

/// <summary>Outcome of an address point import: which file was used and what changed per table.</summary>
public sealed record ImportAddressPointsResponse
{
    /// <summary>Name of the ČÚZK zip the rows were read from.</summary>
    public string? ZipFile { get; set; }

    /// <summary>True when the zip was downloaded during this import, false when it was already on disk.</summary>
    public bool Downloaded { get; set; }

    public RuianImportDiff? MunicipalityParts { get; set; }

    public RuianImportDiff? Streets { get; set; }

    public RuianImportDiff? AddressPoints { get; set; }
}
