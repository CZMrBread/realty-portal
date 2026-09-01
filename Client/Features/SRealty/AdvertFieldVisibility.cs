using System.Collections.Frozen;
using System.Reflection;
using Shared.Shared.Attributes;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.Enums;

namespace Client.Features.SRealty;

/// <summary>Decides which advert fields apply to a category, from the model's RequiredIfValue attributes.</summary>
public static class AdvertFieldVisibility
{
    private static readonly FrozenDictionary<string, AdvertTypeEnum[]> FieldCategories = BuildFieldCategories();

    /// <summary>Whether the field belongs to the chosen category.</summary>
    /// <param name="propertyName">Name of the property on <see cref="SrealityAdvertDto"/>.</param>
    /// <param name="advertType">Chosen category, or null while none is chosen.</param>
    /// <returns>True when no category is chosen or the field is not tied to any category.</returns>
    public static bool AppliesTo(string propertyName, AdvertTypeEnum? advertType)
    {
        return advertType is null
               || !FieldCategories.TryGetValue(propertyName, out var categories)
               || categories.Contains(advertType.Value);
    }

    private static FrozenDictionary<string, AdvertTypeEnum[]> BuildFieldCategories()
    {
        return typeof(SrealityAdvertDto).GetProperties()
            .Select(property => (property.Name, Categories: CategoriesOf(property)))
            .Where(entry => entry.Categories.Length > 0)
            .ToFrozenDictionary(entry => entry.Name, entry => entry.Categories);
    }

    private static AdvertTypeEnum[] CategoriesOf(PropertyInfo property)
    {
        return property.GetCustomAttributes<RequiredIfValueAttribute>()
            .Where(attribute => attribute.PropertyName == nameof(SrealityAdvertDto.AdvertType))
            .SelectMany(attribute => attribute.Values)
            .OfType<AdvertTypeEnum>()
            .Distinct()
            .ToArray();
    }
}
