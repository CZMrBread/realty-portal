using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.Shared.Attributes;
using Shared.SRealty.Enums;
using Shared.SRealty.Enums.Extensions;

namespace Shared.SRealty;

public sealed partial record SrealityAdvertDto : IValidatableObject
{
    [RequiredIfValue(nameof(AdvertRkId), [null])]
    [JsonPropertyName("advert_id")]
    public Guid? AdvertId { get; set; }

    [RequiredIfValue(nameof(AdvertId), [null])]
    [JsonPropertyName("advert_rkid")]
    public string? AdvertRkId { get; set; }

    [RequiredIfValue(nameof(SellerRkId), [null])]
    [JsonPropertyName("seller_id")]
    public Guid? SellerId { get; set; }

    [RequiredIfValue(nameof(SellerId), [null])]
    [JsonPropertyName("seller_rkid")]
    public string? SellerRkId { get; set; }

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
    
    [EnumValue(typeof(ExtraInfoEnum))]
    [JsonPropertyName("extra_info")]
    public ExtraInfoEnum? ExtraInfo { get; set; }

    [JsonPropertyName("user_status")]
    public bool? UserStatus { get; set; }

    [JsonPropertyName("exclusively_at_rk")]
    public bool? ExclusivelyAtRk { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (AdvertType is not null && AdvertSubtype is not null
            && !AdvertSubtype.Value.IsValidSubtype(AdvertType.Value))
        {
            yield return new ValidationResult(
                AdvertType.Value.GetValidSubtypesErrorMessage(AdvertSubtype.Value),
                [nameof(AdvertSubtype)]);
        }
        
        foreach (var result in MutuallyExclusive(AdvertId, AdvertRkId, nameof(AdvertId), nameof(AdvertRkId)))
        {
            yield return result;
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
