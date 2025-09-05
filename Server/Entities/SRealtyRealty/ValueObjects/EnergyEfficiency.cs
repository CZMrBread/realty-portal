using Shared.SRealtyRealty.Enums;

namespace Server.Entities.SRealtyRealty.ValueObjects;

/// <summary>
/// Encapsulates energy efficiency information
/// </summary>
public record EnergyEfficiency
{
    public EnergyRatingEnum? Rating { get; init; }
    public decimal? PerformanceSummary { get; init; }
    public CertificateTypeEnum? CertificateType { get; init; }
    public byte[]? CertificateDocument { get; init; }
    public bool IsLowEnergy { get; init; }
    
    public static EnergyEfficiency Empty => new();
}