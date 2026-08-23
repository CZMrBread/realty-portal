using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Device that releases the heat into the room, such as radiators or floor heating.</summary>
public enum HeatingElementEnum
{
    [LocalizedDisplayName(DisplayNameCz = "WAW", DisplayNameEn = "WAW")]
    WAW = 1,

    [LocalizedDisplayName(DisplayNameCz = "Podlahové topení", DisplayNameEn = "Floor heating")]
    FlorHeating = 2,

    [LocalizedDisplayName(DisplayNameCz = "Radiátory", DisplayNameEn = "Radiators")]
    Radiators = 3,

    [LocalizedDisplayName(DisplayNameCz = "Přímotopy", DisplayNameEn = "Heaters")]
    Heaters = 4,

    [LocalizedDisplayName(DisplayNameCz = "Infrapanel", DisplayNameEn = "Infra panels")]
    InfraPanels = 5,

    [LocalizedDisplayName(DisplayNameCz = "Krbová kamna", DisplayNameEn = "Fireplace stove")]
    FireplaceStove = 6,

    [LocalizedDisplayName(DisplayNameCz = "Krb", DisplayNameEn = "Fireplace")]
    Fireplace = 7,

    [LocalizedDisplayName(DisplayNameCz = "Kotel na tuhá paliva", DisplayNameEn = "Solid fuel boiler")]
    SolidFuelBoiler = 8,

    [LocalizedDisplayName(DisplayNameCz = "Kamna", DisplayNameEn = "Stove")]
    Stove = 9,

    [LocalizedDisplayName(DisplayNameCz = "Klimatizace", DisplayNameEn = "Air conditioner")]
    AirConditioner = 10,

    [LocalizedDisplayName(DisplayNameCz = "Akumulační kamna", DisplayNameEn = "Storage heater")]
    StorageHeater = 11,

    [LocalizedDisplayName(DisplayNameCz = "Jiné", DisplayNameEn = "Other")]
    Other = 12
}