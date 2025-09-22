namespace Shared.SRealtyRealty.Enums;

public enum AdvertPriceUnitEnum
{
    [SRealtyEnums(DisplayNameCz = "za nemovitost", DisplayNameEn = "per property")]
    PerRealty = 1,

    [SRealtyEnums(DisplayNameCz = "za měsíc", DisplayNameEn = "per month")]
    PerMonth = 2,

    [SRealtyEnums(DisplayNameCz = "za m2", DisplayNameEn = "per m2")]
    PerSquareMeter = 3,

    [SRealtyEnums(DisplayNameCz = "za m2/měsíc", DisplayNameEn = "per m2/month")]
    PerSquareMeterPerMonth = 4,

    [SRealtyEnums(DisplayNameCz = "za m2/rok", DisplayNameEn = "per m2/year")]
    PerSquareMeterPerYear = 5,

    [SRealtyEnums(DisplayNameCz = "za rok", DisplayNameEn = "per year")]
    PerYear = 6,

    [SRealtyEnums(DisplayNameCz = "za den", DisplayNameEn = "per day")]
    PerDay = 7,

    [SRealtyEnums(DisplayNameCz = "za hodinu", DisplayNameEn = "per hour")]
    PerHour = 8,

    [SRealtyEnums(DisplayNameCz = "za m2/den", DisplayNameEn = "per m2/day")]
    PerSquareMeterPerDay = 9,

    [SRealtyEnums(DisplayNameCz = "za m2/hodinu", DisplayNameEn = "per m2/hour")]
    PerSquareMeterPerHour = 10
}