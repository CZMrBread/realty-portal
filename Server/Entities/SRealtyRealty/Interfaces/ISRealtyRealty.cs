using System.ComponentModel.DataAnnotations;
using Shared.SRealtyRealty.Enums;

namespace Server.Entities.SRealtyRealty;

public interface ISRealtyRealty
{ 
    public Guid RealtyAgencyId { get; set; }
    public double Price { get; set; }
    public AdvertPriceCurrencyEnum AdvertPriceCurrency { get; set; }
    public AdvertPriceUnitEnum AdvertPriceUnit { get; set; }
    public AdvertLifetimeEnum AdvertLifetime { get; set; }
    public AdvertTypeEnum AdvertType { get; set; }
    public AdvertSubtypeEnum AdvertSubtype { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public int InaccuracyLevel { get; set; }
}