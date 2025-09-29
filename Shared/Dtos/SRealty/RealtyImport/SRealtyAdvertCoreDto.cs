using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.SRealtyRealty.Enums;
using Shared.SRealtyRealty.Interfaces;
using Shared.SRealtyRealty.ValueObjects;
using Shared.Validation;
using Shared.Validation.Attributes;

namespace Shared.Dtos.SRealty.RealtyImport;

[Serializable]
public record SRealtyAdvertCoreDto : IPropertyCore, IValidatableObject
{
    [JsonIgnore] public Guid Id { get; set; }

    [Required]
    [EnumValue(typeof(AdvertFunctionEnum))]
    public AdvertFunctionEnum AdvertFunction { get; set; }

    [Required]
    [EnumValue(typeof(AdvertLifetimeEnum))]
    public AdvertLifetimeEnum AdvertLifetime { get; set; }

    [Required] public double AdvertPrice { get; set; }

    [Required]
    [EnumValue(typeof(AdvertPriceCurrencyEnum))]
    public AdvertPriceCurrencyEnum AdvertPriceCurrency { get; set; }

    [Required]
    [EnumValue(typeof(AdvertPriceUnitEnum))]
    public AdvertPriceUnitEnum AdvertPriceUnit { get; set; }

    [Required]
    [EnumValue(typeof(AdvertTypeEnum))]
    public AdvertTypeEnum AdvertType { get; set; }

    [Required] public string PropertyDescription { get; set; }

    [Required] public string City { get; set; }
    [Required] public int InaccuracyLevel { get; set; }
    public string? AdvertRkId { get; set; }
    public string? RealtyAgentId { get; set; }
    public Guid? RealtyAgentRkId { get; set; }

    public ExclusiveValues<string, Guid?> SellerValues
    {
        get => new(RealtyAgentId, RealtyAgentRkId);
        set
        {
            RealtyAgentId = value.Value1;
            RealtyAgentRkId = value.Value2;
        }
    }

    [Required]
    [EnumValue(typeof(AdvertSubtypeEnum))]
    public AdvertSubtypeEnum AdvertSubtype { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!AdvertSubtype.IsValidSubtype(AdvertType))
        {
            yield return new ValidationResult(
                AdvertType.GetValidSubtypesErrorMessage(AdvertSubtype),
                [nameof(AdvertSubtype)]);
        }
    }
}