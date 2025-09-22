using Shared.SRealtyRealty.Enums;

namespace Shared.SRealtyRealty.Interfaces;

public interface IPropertyConstruction
{
    BuildingConditionEnum? BuildingCondition { get; }
    BuildingTypeEnum? BuildingType { get; }
    ObjectTypeEnum? ObjectType { get; }

    int? AcceptanceYear { get; }
    int? ObjectAge { get; }
    int? ReconstructionYear { get; }

    DateTime? BeginningDate { get; }
    DateTime? FinishDate { get; }

    bool? IsLowEnergy { get; }
    EnergyRatingEnum? EnergyEfficiencyRating { get; }
    CertificateTypeEnum? EnergyPerformanceCertificate { get; }
    double? EnergyPerformanceSummary { get; }

    bool? SolarPanels { get; }
    bool? FtvPanels { get; }
}