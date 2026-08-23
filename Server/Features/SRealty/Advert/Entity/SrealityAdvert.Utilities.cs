using Shared.SRealty.Advert.Enums;

namespace Server.Features.SRealty.Advert.Entity;

public partial class SrealityAdvertEntity
{
    public List<ElectricityTypeEnum>? Electricity { get; set; }
    public CircuitBreakerEnum? CircuitBreaker { get; set; }
    public PhaseCountEnum? PhaseDistribution { get; set; }
    public List<GasTypeEnum>? Gas { get; set; }
    public List<WaterTypeEnum>? Water { get; set; }
    public List<WellTypeEnum>? WellType { get; set; }
    public List<SewerageTypeEnum>? Gully { get; set; }
    public List<HeatingEnum>? Heating { get; set; }
    public List<HeatingElementEnum>? HeatingElement { get; set; }
    public List<HeatingSourceEnum>? HeatingSource { get; set; }
    public List<WaterHeatingSourceEnum>? WaterHeatSource { get; set; }
    public List<TelecommunicationTypeEnum>? Telecommunication { get; set; }
    public List<InternetConnectionTypeEnum>? InternetConnectionType { get; set; }
    public string? InternetConnectionProvider { get; set; }
    public int? InternetConnectionSpeed { get; set; }
}
