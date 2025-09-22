using System.Reflection;

namespace Shared.SRealtyRealty.Enums;

public static class SRealtyEnumsExtension
{
    public static string GetDisplayNameCz<T>(this T enumValue) where T : struct, Enum
    {
        var attribute = enumValue.GetSRealtyAttribute();
        return attribute?.DisplayNameCz ?? enumValue.ToString();
    }

    public static string GetDisplayNameEn<T>(this T enumValue) where T : struct, Enum
    {
        var attribute = enumValue.GetSRealtyAttribute();
        return attribute?.DisplayNameEn ?? enumValue.ToString();
    }

    public static string GetDescriptionCz<T>(this T enumValue) where T : struct, Enum
    {
        var attribute = enumValue.GetSRealtyAttribute();
        return attribute?.DescriptionCz ?? string.Empty;
    }

    public static string GetDescriptionEn<T>(this T enumValue) where T : struct, Enum
    {
        var attribute = enumValue.GetSRealtyAttribute();
        return attribute?.DescriptionEn ?? string.Empty;
    }

    public static string GetIcon<T>(this T enumValue) where T : struct, Enum
    {
        var attribute = enumValue.GetSRealtyAttribute();
        return attribute?.Icon ?? string.Empty;
    }

    public static bool GetIsActive<T>(this T enumValue) where T : struct, Enum
    {
        var attribute = enumValue.GetSRealtyAttribute();
        return attribute?.IsActive ?? true;
    }

    public static SRealtyEnumsAttribute? GetSRealtyAttribute<T>(this T enumValue) where T : struct, Enum
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        return field?.GetCustomAttribute<SRealtyEnumsAttribute>();
    }

    public static string GetLocalizedDisplayName<T>(this T enumValue, string cultureName = "cs-CZ")
        where T : struct, Enum
    {
        return cultureName.StartsWith("cs", StringComparison.OrdinalIgnoreCase)
            ? enumValue.GetDisplayNameCz()
            : enumValue.GetDisplayNameEn();
    }

    public static string GetLocalizedDescription<T>(this T enumValue, string cultureName = "cs-CZ")
        where T : struct, Enum
    {
        return cultureName.StartsWith("cs", StringComparison.OrdinalIgnoreCase)
            ? enumValue.GetDescriptionCz()
            : enumValue.GetDescriptionEn();
    }
}