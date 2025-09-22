using Shared.SRealtyRealty.Enums;
using Shared.SRealtyRealty.ValueObjects;
using NetTopologySuite.Geometries;

namespace Shared.SRealtyRealty.Interfaces;

public interface IPropertyCore
{
    string AdvertRkId { get; init; }

    AdvertFunctionEnum AdvertFunction { get; }
    AdvertLifetimeEnum AdvertLifetime { get; }
    AdvertTypeEnum AdvertType { get; }

    // Price primitive properties
    double PriceAmount { get; }
    AdvertPriceCurrencyEnum AdvertPriceCurrency { get; }
    AdvertPriceUnitEnum AdvertPriceUnit { get; }
    bool IsPriceNegotiable { get; }
    string? PriceNote { get; }
    string? PriceNoteEn { get; }
    string? PriceNoteRu { get; }

    // Description primitive properties
    string PropertyDescription { get; }
    string? PropertyDescriptionEn { get; }
    string? PropertyDescriptionRu { get; }

    // Location primitive properties
    string City { get; }
    int InaccuracyLevel { get; }
    string? CityPart { get; }
    string? Street { get; }
    string? DescriptiveNumber { get; }
    string? OrientationNumber { get; }
    double? Latitude { get; }
    double? Longitude { get; }
    int? RuianId { get; }
    int? RuianLevel { get; }
    int? UirId { get; }
    int? UirLevel { get; }
    Point? Coordinates { get; }

    // Computed ValueObject properties
    Price Price { get; }
    PropertyDescription Description { get; }
    Location Location { get; }
}