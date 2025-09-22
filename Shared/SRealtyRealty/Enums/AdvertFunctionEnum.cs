namespace Shared.SRealtyRealty.Enums;

public enum AdvertFunctionEnum
{
    [SRealtyEnums(DisplayNameCz = "Prodej", DisplayNameEn = "Sell", DescriptionCz = "Prodej nemovitosti",
        DescriptionEn = "Property sale", Icon = "bi-currency-dollar")]
    Sell = 1,

    [SRealtyEnums(DisplayNameCz = "Pronájem", DisplayNameEn = "Rent", DescriptionCz = "Pronájem nemovitosti",
        DescriptionEn = "Property rental", Icon = "bi-key")]
    Rent = 2,

    [SRealtyEnums(DisplayNameCz = "Aukce", DisplayNameEn = "Auction",
        DescriptionCz = "Prodej nemovitosti aukčním způsobem", DescriptionEn = "Property auction sale",
        Icon = "bi-hammer")]
    Auction = 3,

    [SRealtyEnums(DisplayNameCz = "Podíly", DisplayNameEn = "Shares", DescriptionCz = "Prodej podílů nemovitosti",
        DescriptionEn = "Property shares sale", Icon = "bi-pie-chart")]
    Shares = 4
}