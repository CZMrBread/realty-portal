using System.Buffers.Text;
using Shared.SRealtyRealty.Enums;

namespace Shared.SRealtyRealty.Interfaces;

public interface IPropertyEnergyPerformance
{
    EnergyRatingEnum? EnergyEfficiencyRating { get; set; }
    EnergyPerformanceCertificateEnum? EnergyEfficiencyCertificate { get; set; }
    double? EnergyPerformanceSummary { get; set; }
}