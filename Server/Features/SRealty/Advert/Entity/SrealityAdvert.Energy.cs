using Shared.SRealty.Advert.Enums;

namespace Server.Features.SRealty.Advert.Entity;

public partial class SrealityAdvertEntity
{
    public EnergyRatingEnum? EnergyEfficiencyRating { get; set; }
    public EnergyPerformanceCertificateEnum? EnergyPerformanceCertificate { get; set; }
    public double? EnergyPerformanceSummary { get; set; }
    public bool? AdvertLowEnergy { get; set; }
}
