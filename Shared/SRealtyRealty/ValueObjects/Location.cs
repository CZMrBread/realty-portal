

using System.Drawing;

namespace Shared.SRealtyRealty.ValueObjects;

public record Location
{
    public string City { get; init; }

    public int InaccuracyLevel { get; init; }

    public string? CityPart { get; init; }

    public string? Street { get; init; }

    public string? DescriptiveNumber { get; init; }

    public string? OrientationNumber { get; init; }

    public double? Latitude { get; init; }

    public double? Longitude { get; init; }

    public int? RuianId { get; init; }

    public int? RuianLevel { get; init; }

    public int? UirId { get; init; }

    public int? UirLevel { get; init; }

    public Point? Coordinates { get; init; }

    public Location(string city, int inaccuracyLevel)
    {
        City = city;
        InaccuracyLevel = inaccuracyLevel;
    }
}