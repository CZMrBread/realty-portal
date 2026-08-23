using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.Shared.Attributes;
using Shared.SRealty.Advert.Enums;
using Shared.SRealty.Advert.Enums.Extensions;

namespace Shared.SRealty.Advert;

/// <summary>
/// Advert exactly as it is exchanged with a real estate agency over the API. Every field is nullable so that
/// a missing value can be told apart from an empty one, and the required ones are enforced by validation
/// attributes rather than by the type system. The record is split into partial files by topic; this one holds
/// the identification of the advert and the cross-field validation rules.
/// </summary>
public sealed partial record SrealityAdvertDto : IValidatableObject
{
    /// <summary>Identifier assigned by the portal. Written to the client only, never read from an incoming payload.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
    [JsonPropertyName("advert_id")]
    public Guid? AdvertId { get; set; }

    /// <summary>Key of the advert in the agency own system. Unique within one agency, not globally.</summary>
    [JsonPropertyName("advert_rkid")]
    public string? AdvertRkId { get; set; }

    /// <summary>Selling agent addressed by portal identifier. Exactly one of SellerId and SellerRkId has to be filled in.</summary>
    [RequiredIfValue(nameof(SellerRkId), [null])]
    [JsonPropertyName("seller_id")]
    public Guid? SellerId { get; set; }

    /// <summary>Selling agent addressed by the key the agency uses for them. Unique within one agency, not globally.</summary>
    [RequiredIfValue(nameof(SellerId), [null])]
    [JsonPropertyName("seller_rkid")]
    public string? SellerRkId { get; set; }

    /// <summary>Reference code of the advert as the agency shows it to its own clients.</summary>
    [JsonPropertyName("advert_code")]
    public string? AdvertCode { get; set; }

    [Required]
    [EnumValue(typeof(AdvertFunctionEnum))]
    [JsonPropertyName("advert_function")]
    public AdvertFunctionEnum? AdvertFunction { get; set; }

    [Required]
    [EnumValue(typeof(AdvertLifetimeEnum))]
    [JsonPropertyName("advert_lifetime")]
    public AdvertLifetimeEnum? AdvertLifetime { get; set; }

    [Required]
    [EnumValue(typeof(AdvertTypeEnum))]
    [JsonPropertyName("advert_type")]
    public AdvertTypeEnum? AdvertType { get; set; }

    [Required]
    [EnumValue(typeof(AdvertSubtypeEnum))]
    [JsonPropertyName("advert_subtype")]
    public AdvertSubtypeEnum? AdvertSubtype { get; set; }

    [RequiredIfValue(nameof(AdvertType), [AdvertTypeEnum.House])]
    [EnumValue(typeof(AdvertRoomCountEnum))]
    [JsonPropertyName("advert_room_count")]
    public AdvertRoomCountEnum? AdvertRoomCount { get; set; }
    
    /// <summary>Marks an advert that is reserved or already sold but should stay visible.</summary>
    [EnumValue(typeof(ExtraInfoEnum))]
    [JsonPropertyName("extra_info")]
    public ExtraInfoEnum? ExtraInfo { get; set; }

    [JsonPropertyName("user_status")]
    public bool? UserStatus { get; set; }

    /// <summary>Whether the agency holds an exclusive mandate for the property.</summary>
    [JsonPropertyName("exclusively_at_rk")]
    public bool? ExclusivelyAtRk { get; set; }

    /// <summary>
    /// Rules that span more than one field and therefore cannot be expressed by a single attribute.
    /// </summary>
    /// <param name="validationContext">Context supplied by the validation infrastructure.</param>
    /// <returns>One result per broken rule; an empty sequence when the advert is consistent.</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (AdvertType is not null && AdvertSubtype is not null
            && !AdvertSubtype.Value.IsValidSubtype(AdvertType.Value))
        {
            yield return new ValidationResult(
                AdvertType.Value.GetValidSubtypesErrorMessage(AdvertSubtype.Value),
                [nameof(AdvertSubtype)]);
        }

        foreach (var result in MutuallyExclusive(SellerId, SellerRkId, nameof(SellerId), nameof(SellerRkId)))
        {
            yield return result;
        }
        
        foreach (var result in BothOrNeither(LocalityLatitude, LocalityLongitude,
                     nameof(LocalityLatitude), nameof(LocalityLongitude)))
        {
            yield return result;
        }

        foreach (var result in BothOrNeither(LocalityRuian, LocalityRuianLevel,
                     nameof(LocalityRuian), nameof(LocalityRuianLevel)))
        {
            yield return result;
        }

        foreach (var result in BothOrNeither(LocalityUir, LocalityUirLevel,
                     nameof(LocalityUir), nameof(LocalityUirLevel)))
        {
            yield return result;
        }

        if (ApartmentNumber is not null && AdvertType != AdvertTypeEnum.Flat)
        {
            yield return new ValidationResult(
                "Číslo bytové jednotky lze zadat pouze u kategorie Byty.",
                [nameof(ApartmentNumber)]);
        }

        if (AdvertFunction == AdvertFunctionEnum.Sell
            && Ownership == OwnershipTypeEnum.Cooperative
            && Personal is null)
        {
            yield return new ValidationResult(
                "Převod do osobního vlastnictví je povinný u prodeje družstevního bytu.",
                [nameof(Personal)]);
        }
    }

    /// <summary>Reports an error when both values are filled in, for fields where only one of the two may be used.</summary>
    private static IEnumerable<ValidationResult> MutuallyExclusive(
        object? first, object? second, string firstName, string secondName)
    {
        if (first is not null && second is not null)
        {
            yield return new ValidationResult(
                $"{firstName} a {secondName} nelze zadat současně.",
                [firstName, secondName]);
        }
    }

    /// <summary>Reports an error when only one of a pair of values is filled in, for fields that are meaningful only together.</summary>
    private static IEnumerable<ValidationResult> BothOrNeither(
        object? first, object? second, string firstName, string secondName)
    {
        if (first is null != second is null)
        {
            yield return new ValidationResult(
                $"{firstName} a {secondName} musí být zadány společně.",
                [firstName, secondName]);
        }
    }
}
