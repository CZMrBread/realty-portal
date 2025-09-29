using Shared.SRealtyRealty.Enums;

namespace Shared.SRealtyRealty.Interfaces;

public interface IPropertyUtilities
{
    CircuitBreakerEnum? CircuitBreaker { get; set; }
    List<ElectricityTypeEnum>? Electricity { get; set; } 
    PhaseCountEnum? PhaseDistribution { get; set; }
    bool FtvPanels { get; set; } 
    List<GasTypeEnum>? Gas { get; set; }
    List<SewerageTypeEnum>? Gully { get; set; }
    List<HeatingEnum>? Heating { get; set; }
    List<HeatingElementEnum>? HeatingElement { get; set; }
    List<HeatingSourceEnum>? HeatingSource { get; set; }
    string? InternetConnectionProvider { get; set; }
    List<InternetConnectionTypeEnum>? InternetConnectionType { get; set; }
    int? InternetConnectionSpeed { get; set; }
    List<TelecommunicationTypeEnum>? Telecommunication { get; set; }
    List<WaterTypeEnum>? Water { get; set; }
    List<WaterHeatingSourceEnum>? WaterHeatingSource { get; set; }
    List<WellTypeEnum>? WellType { get; set; }
}