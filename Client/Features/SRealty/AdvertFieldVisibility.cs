using System.Collections.Frozen;
using System.Reflection;
using Shared.Shared.Attributes;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.Enums;

namespace Client.Features.SRealty;

/// <summary>
/// Decides which advert fields the form is worth showing for the category the advert is set to. The rule is
/// read off the model itself: a property carrying RequiredIfValue against AdvertType belongs to the categories
/// listed there and is left out for the rest, and a property carrying no such attribute belongs everywhere.
/// Nothing is hard coded here, so adding a category to an attribute is enough to make the field appear.
/// </summary>
public static class AdvertFieldVisibility
{
    private static readonly FrozenDictionary<string, AdvertTypeEnum[]> FieldCategories = BuildFieldCategories();

    /// <summary>Tells whether the field belongs to the chosen category.</summary>
    /// <param name="propertyName">Name of the property on <see cref="SrealityAdvertDto"/>.</param>
    /// <param name="advertType">Category the advert is set to, or null while none is chosen.</param>
    /// <returns>True while no category is chosen, or when the field is not tied to a category at all.</returns>
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
