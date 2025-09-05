using NetTopologySuite.Geometries;
using Shared.SRealtyRealty.Enums;

namespace Server.Entities.SRealtyRealty.Interfaces;

/// <summary>
/// Defines geographical and address location data
/// </summary>
public interface ILocationData
{
    Point? Location { get; }
    int? RuianCode { get; }
    RuianLevelEnum? RuianLevel { get; }
    string? City { get; }
    string? CityPart { get; }
    string? Street { get; }
    string? OrientationNumber { get; }
    string? DescriptiveNumber { get; }
}