using System.Reflection;
using Shared.Shared.Attributes;

namespace Shared.SRealty.Enums.Extensions;

public static class ValidForTypeExtension
{
    public static TType? GetValidForType<TEnum, TType>(this TEnum enumValue)
        where TEnum : struct, Enum
        where TType : struct, Enum
    {
        var attribute = enumValue.GetValidForTypeAttribute<TEnum, TType>();
        return attribute?.Type;
    }

    private static ValidForTypeAttribute<TType>? GetValidForTypeAttribute<TEnum, TType>(this TEnum enumValue)
        where TEnum : struct, Enum
        where TType : struct, Enum
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        return field?.GetCustomAttribute<ValidForTypeAttribute<TType>>();
    }

    public static bool IsValidForType<TEnum, TType>(this TEnum enumValue, TType type)
        where TEnum : struct, Enum
        where TType : struct, Enum
    {
        return enumValue.GetValidForType<TEnum, TType>()?.Equals(type) == true;
    }
}