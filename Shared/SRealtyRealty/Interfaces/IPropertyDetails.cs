using Shared.SRealtyRealty.Enums;

namespace Shared.SRealtyRealty.Interfaces;

public interface IPropertyDetails
{
    AdvertSubtypeEnum AdvertSubtype { get; }
    AdvertRoomCountEnum? AdvertRoomCount { get; }

    int? ApartmentNumber { get; }
    int? FloorNumber { get; }
    int? Floors { get; }
    int? UndergroundFloors { get; }

    FlatClassEnum? FlatClass { get; }
    ObjectKindEnum? ObjectKind { get; }
    ObjectLocationEnum? ObjectLocation { get; }
}