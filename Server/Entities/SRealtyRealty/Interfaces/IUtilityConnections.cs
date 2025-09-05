using Shared.SRealtyRealty.Enums;

namespace Server.Entities.SRealtyRealty.Interfaces;

/// <summary>
/// Defines utility and infrastructure connections
/// </summary>
public interface IUtilityConnections
{
    ElectricityTypeEnum? ElectricityType { get; }
    GasTypeEnum? GasType { get; }
    WaterTypeEnum? WaterType { get; }
    SewerageTypeEnum? SewerageType { get; }
    HeatingTypeEnum? HeatingType { get; }
    TelecommunicationTypeEnum? TelecommunicationType { get; }
}