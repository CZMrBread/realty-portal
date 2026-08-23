using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>What the advert offers: a sale, a rental, an auction or a transfer of ownership shares.</summary>
public enum AdvertFunctionEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Prodej", DisplayNameEn = "Sell", DescriptionCz = "Prodej nemovitosti",
        DescriptionEn = "Property sale", Icon = "bi-currency-dollar")]
    Sell = 1,

    [LocalizedDisplayName(DisplayNameCz = "Pronájem", DisplayNameEn = "Rent", DescriptionCz = "Pronájem nemovitosti",
        DescriptionEn = "Property rental", Icon = "bi-key")]
    Rent = 2,

    [LocalizedDisplayName(DisplayNameCz = "Aukce", DisplayNameEn = "Auction",
        DescriptionCz = "Prodej nemovitosti aukčním způsobem", DescriptionEn = "Property auction sale",
        Icon = "bi-hammer")]
    Auction = 3,

    [LocalizedDisplayName(DisplayNameCz = "Podíly", DisplayNameEn = "Shares", DescriptionCz = "Prodej podílů nemovitosti",
        DescriptionEn = "Property shares sale", Icon = "bi-pie-chart")]
    Shares = 4
}