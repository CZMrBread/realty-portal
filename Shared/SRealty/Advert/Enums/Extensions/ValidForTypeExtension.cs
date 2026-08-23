using System.Reflection;
using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums.Extensions;

/// <summary>Reads the parent category that ValidForTypeAttribute declares on an enum member.</summary>
public static class ValidForTypeExtension
{
    /// <summary>Parent category declared for the member, or null when it declares none.</summary>
    public static TType? GetValidForType<TEnum, TType>(this TEnum enumValue)
        where TEnum : struct, Enum
        where TType : struct, Enum
    {
        var attribute = enumValue.GetValidForTypeAttribute<TEnum, TType>();
        return attribute?.Type;
    }

    /// <summary>Attribute declared on the member, or null when it carries none.</summary>
    private static ValidForTypeAttribute<TType>? GetValidForTypeAttribute<TEnum, TType>(this TEnum enumValue)
        where TEnum : struct, Enum
        where TType : struct, Enum
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        return field?.GetCustomAttribute<ValidForTypeAttribute<TType>>();
    }

    /// <summary>Tells whether the member belongs under the given parent category.</summary>
    public static bool IsValidForType<TEnum, TType>(this TEnum enumValue, TType type)
        where TEnum : struct, Enum
        where TType : struct, Enum
    {
        return enumValue.GetValidForType<TEnum, TType>()?.Equals(type) == true;
    }
}