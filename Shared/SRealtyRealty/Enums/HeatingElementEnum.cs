namespace Shared.SRealtyRealty.Enums;

public enum HeatingElementEnum
{
    [SRealtyEnums(DisplayNameCz = "WAW", DisplayNameEn = "WAW")]
    WAW = 1,
    [SRealtyEnums(DisplayNameCz = "Podlahové topení", DisplayNameEn = "Floor heating")]
    FlorHeating = 2,
    [SRealtyEnums(DisplayNameCz = "Radiátory", DisplayNameEn = "Radiators")]
    Radiators = 3,
    [SRealtyEnums(DisplayNameCz = "Přímotopy", DisplayNameEn = "Heaters")]
    Heaters = 4,
    [SRealtyEnums(DisplayNameCz = "Infrapanel", DisplayNameEn = "Infra panels")]
    InfraPanels = 5,
    [SRealtyEnums(DisplayNameCz = "Krbová kamna", DisplayNameEn = "Fireplace stove")]
    FireplaceStove = 6,
    [SRealtyEnums(DisplayNameCz = "Krb", DisplayNameEn = "Fireplace")]
    Fireplace = 7,
    [SRealtyEnums(DisplayNameCz = "Kotel na tuhá paliva", DisplayNameEn = "Solid fuel boiler")]
    SolidFuelBoiler = 8,
    [SRealtyEnums(DisplayNameCz = "Kamna", DisplayNameEn = "Stove")]
    Stove = 9,
    [SRealtyEnums(DisplayNameCz = "Klimatizace", DisplayNameEn = "Air conditioner")]
    AirConditioner = 10,
    [SRealtyEnums(DisplayNameCz = "Akumulační kamna", DisplayNameEn = "Storage heater")]
    StorageHeater = 11,
    [SRealtyEnums(DisplayNameCz = "Jiné", DisplayNameEn = "Other")]
    Other = 12
}