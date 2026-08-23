using System.Reflection;
using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums.Extensions;

/// <summary>Reads the texts that LocalizedDisplayNameAttribute puts on enum members. Every method falls back to the member name or an empty string when the attribute is missing.</summary>
public static class SRealtyEnumsExtension
{
    /// <summary>Czech label of the member, or its name when none is declared.</summary>
    public static string GetDisplayNameCz<T>(this T enumValue) where T : struct, Enum
    {
        var attribute = enumValue.GetSRealtyAttribute();
        return attribute?.DisplayNameCz ?? enumValue.ToString();
    }

    /// <summary>English label of the member, or its name when none is declared.</summary>
    public static string GetDisplayNameEn<T>(this T enumValue) where T : struct, Enum
    {
        var attribute = enumValue.GetSRealtyAttribute();
        return attribute?.DisplayNameEn ?? enumValue.ToString();
    }

    /// <summary>Longer Czech description of the member, or an empty string.</summary>
    public static string GetDescriptionCz<T>(this T enumValue) where T : struct, Enum
    {
        var attribute = enumValue.GetSRealtyAttribute();
        return attribute?.DescriptionCz ?? string.Empty;
    }

    /// <summary>Longer English description of the member, or an empty string.</summary>
    public static string GetDescriptionEn<T>(this T enumValue) where T : struct, Enum
    {
        var attribute = enumValue.GetSRealtyAttribute();
        return attribute?.DescriptionEn ?? string.Empty;
    }

    /// <summary>Icon declared for the member, or an empty string.</summary>
    public static string GetIcon<T>(this T enumValue) where T : struct, Enum
    {
        var attribute = enumValue.GetSRealtyAttribute();
        return attribute?.Icon ?? string.Empty;
    }

    /// <summary>Whether the member should still be offered to users. Members without the attribute count as active.</summary>
    public static bool GetIsActive<T>(this T enumValue) where T : struct, Enum
    {
        var attribute = enumValue.GetSRealtyAttribute();
        return attribute?.IsActive ?? true;
    }

    /// <summary>Attribute declared on the member, or null when it carries none.</summary>
    public static LocalizedDisplayNameAttribute? GetSRealtyAttribute<T>(this T enumValue) where T : struct, Enum
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        return field?.GetCustomAttribute<LocalizedDisplayNameAttribute>();
    }

    /// <summary>Label of the member in the requested culture.</summary>
    /// <param name="cultureName">Culture to pick the wording for; anything other than Czech falls back to English.</param>
    public static string GetLocalizedDisplayName<T>(this T enumValue, string cultureName = "cs-CZ")
        where T : struct, Enum
    {
        return cultureName.StartsWith("cs", StringComparison.OrdinalIgnoreCase)
            ? enumValue.GetDisplayNameCz()
            : enumValue.GetDisplayNameEn();
    }

    /// <summary>Description of the member in the requested culture.</summary>
    /// <param name="cultureName">Culture to pick the wording for; anything other than Czech falls back to English.</param>
    public static string GetLocalizedDescription<T>(this T enumValue, string cultureName = "cs-CZ")
        where T : struct, Enum
    {
        return cultureName.StartsWith("cs", StringComparison.OrdinalIgnoreCase)
            ? enumValue.GetDescriptionCz()
            : enumValue.GetDescriptionEn();
    }
}