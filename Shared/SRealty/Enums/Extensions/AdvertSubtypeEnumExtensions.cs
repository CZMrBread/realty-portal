namespace Shared.SRealty.Enums.Extensions;

public static class AdvertSubtypeEnumExtensions
{
    public static bool IsValidSubtype(this AdvertSubtypeEnum subtype, AdvertTypeEnum type)
    {
        return subtype.IsValidForType(type);
    }

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