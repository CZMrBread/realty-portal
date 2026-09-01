namespace Shared.SRealty.Advert.Enums.Extensions;

/// <summary>Checks whether a property subtype belongs under a given property type.</summary>
public static class AdvertSubtypeEnumExtensions
{
    /// <summary>Whether the subtype is allowed for the given type.</summary>
    public static bool IsValidSubtype(this AdvertSubtypeEnum subtype, AdvertTypeEnum type)
    {
        return subtype.IsValidForType(type);
    }

    /// <summary>Validation message listing every subtype allowed for the type, with numeric values.</summary>
    public static string GetValidSubtypesErrorMessage(this AdvertTypeEnum type, AdvertSubtypeEnum chosenSubtype)
    {
        var validSubtypes = Enum.GetValues<AdvertSubtypeEnum>()
            .Where(subtype => subtype.IsValidForType(type))
            .Select(subtype => $"{subtype}={((int)subtype)}")
            .ToArray();

        var chosenSubtypeWithValue = $"{chosenSubtype}={((int)chosenSubtype)}";
        var typeWithValue = $"{type}={((int)type)}";

        return validSubtypes.Length > 0
            ? $"Invalid subtype '{chosenSubtypeWithValue}' for type '{typeWithValue}'. Valid subtypes for '{typeWithValue}': [{string.Join(", ", validSubtypes)}]"
            : $"Invalid subtype '{chosenSubtypeWithValue}' for type '{typeWithValue}'. No valid subtypes found for this type.";
    }
}