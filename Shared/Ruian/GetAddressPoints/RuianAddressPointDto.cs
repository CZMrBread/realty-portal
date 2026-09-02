namespace Shared.Ruian.GetAddressPoints;

/// <summary>An address point of the RUIAN register.</summary>
public sealed record RuianAddressPointDto
{
    public int Code { get; set; }

    public int MunicipalityCode { get; set; }

    public int MunicipalityPartCode { get; set; }

    public int? StreetCode { get; set; }

    public HouseNumberTypeEnum HouseNumberType { get; set; }

    public int HouseNumber { get; set; }

    public int? OrientationNumber { get; set; }

    public string? OrientationLetter { get; set; }

    public string? PostalCode { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    /// <summary>The number as written on the house: "č.p. 12/5a" or "č.ev. 7".</summary>
    public string Label
    {
        get
        {
            var kind = HouseNumberType == HouseNumberTypeEnum.Registration ? "č.ev." : "č.p.";
            var orientation = OrientationNumber is { } number ? $"/{number}{OrientationLetter}" : string.Empty;
            return $"{kind} {HouseNumber}{orientation}";
        }
    }
}
