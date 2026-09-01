using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Unit the price applies to: the whole property, a month, a square metre per month, etc.</summary>
public enum AdvertPriceUnitEnum
{
    [LocalizedDisplayName(DisplayNameCz = "za nemovitost", DisplayNameEn = "per property")]
    PerRealty = 1,

    [LocalizedDisplayName(DisplayNameCz = "za měsíc", DisplayNameEn = "per month")]
    PerMonth = 2,

    [LocalizedDisplayName(DisplayNameCz = "za m2", DisplayNameEn = "per m2")]
    PerSquareMeter = 3,

    [LocalizedDisplayName(DisplayNameCz = "za m2/měsíc", DisplayNameEn = "per m2/month")]
    PerSquareMeterPerMonth = 4,

    [LocalizedDisplayName(DisplayNameCz = "za m2/rok", DisplayNameEn = "per m2/year")]
    PerSquareMeterPerYear = 5,

    [LocalizedDisplayName(DisplayNameCz = "za rok", DisplayNameEn = "per year")]
    PerYear = 6,

    [LocalizedDisplayName(DisplayNameCz = "za den", DisplayNameEn = "per day")]
    PerDay = 7,

    [LocalizedDisplayName(DisplayNameCz = "za hodinu", DisplayNameEn = "per hour")]
    PerHour = 8,

    [LocalizedDisplayName(DisplayNameCz = "za m2/den", DisplayNameEn = "per m2/day")]
    PerSquareMeterPerDay = 9,

    [LocalizedDisplayName(DisplayNameCz = "za m2/hodinu", DisplayNameEn = "per m2/hour")]
    PerSquareMeterPerHour = 10
}