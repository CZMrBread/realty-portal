using Shared.SRealtyRealty.Enums;

namespace Server.Entities.SRealtyRealty.Interfaces;

/// <summary>
/// Defines the contract for SReality property entities
/// </summary>
public interface ISRealityProperty : ISRealtyRealty
{
    AdvertFunctionEnum AdvertFunction { get; }
    
    // Navigation properties
    IReadOnlyCollection<string> GetKeywords();
    void SetKeywords(IEnumerable<string> keywords);
}