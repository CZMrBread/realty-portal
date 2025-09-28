namespace Shared.SRealtyRealty.Interfaces;

public interface IPropertyLocation
{
    string City { get; set; }
    double? Altitude { get; set; }
    double? Latitude { get; set; }
    int? RuianId { get; set; }
    int? RuianLevel { get; set; }
    int? UirId { get; set; }
    int? UirLevel { get; set; }
    string? CityPart { get; set; }
    string? OrientationNumber { get; set; }
    string? Street { get; set; }
    string? HouseNumber { get; set; }
    
}