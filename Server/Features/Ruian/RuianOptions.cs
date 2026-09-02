namespace Server.Features.Ruian;

/// <summary>Settings of the RUIAN feature, bound from the "Ruian" section.</summary>
public sealed class RuianOptions
{
    public const string SectionName = "Ruian";

    /// <summary>Directory the ČÚZK address point zips are kept in; a relative path is taken from the content root.</summary>
    public string AddressDataPath { get; set; } = "Features/Ruian/Data/Addresses";

    /// <summary>Secret the address point import route requires in its header; unset disables the route.</summary>
    public string? ImportKey { get; set; }
}
