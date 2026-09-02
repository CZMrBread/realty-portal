using Shared.Ruian;

namespace Server.Features.Ruian.Entity;

/// <summary>An address point (adresni misto) of the RUIAN address register, keyed by its RUIAN code.</summary>
public class RuianAddressPointEntity
{
    public int Code { get; set; }

    public int MunicipalityCode { get; set; }
    public RuianMunicipalityEntity Municipality { get; set; } = null!;

    public int MunicipalityPartCode { get; set; }
    public RuianMunicipalityPartEntity MunicipalityPart { get; set; } = null!;

    /// <summary>City district (mestsky obvod or mestska cast) of a statutory city, such as Praha 1; null elsewhere.</summary>
    public int? CityDistrictCode { get; set; }
    public string? CityDistrictName { get; set; }

    /// <summary>Null where the municipality has no named streets.</summary>
    public int? StreetCode { get; set; }
    public RuianStreetEntity? Street { get; set; }

    public HouseNumberTypeEnum HouseNumberType { get; set; }
    public int HouseNumber { get; set; }
    public int? OrientationNumber { get; set; }
    public string? OrientationLetter { get; set; }
    public required string PostalCode { get; set; }

    /// <summary>WGS84 position of the definition point; null for the few address points ČÚZK has not located.</summary>
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
