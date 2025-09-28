using System.Drawing;
using Shared.SRealtyRealty.Enums;
using Shared.SRealtyRealty.ValueObjects;

namespace Shared.SRealtyRealty.Interfaces;

public interface IPropertyCore
{
    AdvertFunctionEnum AdvertFunction { get; set; }
    AdvertLifetimeEnum AdvertLifetime { get; set; }
    double AdvertPrice { get; set; }
    AdvertPriceCurrencyEnum AdvertPriceCurrency { get; set; }
    AdvertPriceUnitEnum AdvertPriceUnit { get; set; }
    AdvertTypeEnum AdvertType { get; set; }
    string PropertyDescription { get; set; }
    string City { get; set; }
    int InaccuracyLevel { get; set; }
    string? AdvertRkId { get; set; }
    
    string? RealtyAgentId { get; set; }
    Guid? RealtyAgentRkId { get; set; }
    AdvertSubtypeEnum AdvertSubtype { get; set; }

    Price Price
    {
        get => new Price(AdvertPrice, AdvertPriceCurrency, AdvertPriceUnit);
        set
        {
            AdvertPrice = value.Amount;
            AdvertPriceCurrency = value.Currency;
            AdvertPriceUnit = value.Unit;
        }
    }
    PropertyDescription Description
    {
        get => new PropertyDescription(PropertyDescription);
        set => PropertyDescription = value.Description;
    }
}