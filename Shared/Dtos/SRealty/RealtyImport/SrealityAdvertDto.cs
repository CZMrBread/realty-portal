using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.SRealtyRealty.Enums;

namespace Shared.Dtos.SRealty.RealtyImport;

public sealed record SrealityAdvertDto : IValidatableObject
{
    [Required]
    [EnumDataType(typeof(AdvertFunctionEnum))]
    public AdvertFunctionEnum AdvertFunction { get; set; }
    
    [Required]
    [EnumDataType(typeof(AdvertLifetimeEnum))]
    public AdvertLifetimeEnum AdvertLifetime { get; set; }

    [Required]
    [EnumDataType(typeof(AdvertTypeEnum))]
    public AdvertTypeEnum AdvertType { get; set; }

    [Required]
    [EnumDataType(typeof(AdvertSubtypeEnum))]
    public AdvertSubtypeEnum AdvertSubtype { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errors = new List<ValidationResult>();
        
        errors.AddRange(AdvertType switch
        {
            AdvertTypeEnum.Flat => ValidateFlat(),
            AdvertTypeEnum.House => ValidateHouse(),
            AdvertTypeEnum.Land => ValidateLand(),
            AdvertTypeEnum.Commercial => ValidateCommercial(),
            AdvertTypeEnum.Other => ValidateOther(),
            _ => throw new ArgumentOutOfRangeException()
        });
        
        return errors;
    }

    private IEnumerable<ValidationResult> ValidateFlat()
    {
        // Use existing ValidForType validation system
        if (!AdvertSubtype.IsValidForType(AdvertTypeEnum.Flat))
        {
            yield return new ValidationResult(
                $"Subtype '{AdvertSubtype}' is not valid for Flat advertisements",
                new[] { nameof(AdvertSubtype) });
        }
    }

    private IEnumerable<ValidationResult> ValidateHouse()
    {
        if (!AdvertSubtype.IsValidForType(AdvertTypeEnum.House))
        {
            yield return new ValidationResult(
                $"Subtype '{AdvertSubtype}' is not valid for House advertisements",
                new[] { nameof(AdvertSubtype) });
        }
    }

    private IEnumerable<ValidationResult> ValidateLand()
    {
        if (!AdvertSubtype.IsValidForType(AdvertTypeEnum.Land))
        {
            yield return new ValidationResult(
                $"Subtype '{AdvertSubtype}' is not valid for Land advertisements",
                new[] { nameof(AdvertSubtype) });
        }
    }

    private IEnumerable<ValidationResult> ValidateCommercial()
    {
        if (!AdvertSubtype.IsValidForType(AdvertTypeEnum.Commercial))
        {
            yield return new ValidationResult(
                $"Subtype '{AdvertSubtype}' is not valid for Commercial advertisements",
                new[] { nameof(AdvertSubtype) });
        }
    }

    private IEnumerable<ValidationResult> ValidateOther()
    {
        if (!AdvertSubtype.IsValidForType(AdvertTypeEnum.Other))
        {
            yield return new ValidationResult(
                $"Subtype '{AdvertSubtype}' is not valid for Other advertisements",
                new[] { nameof(AdvertSubtype) });
        }
    }
};