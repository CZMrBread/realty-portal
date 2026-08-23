namespace Shared.Shared.Attributes;

/// <summary>
/// Ties an enum member to the member of another enum it belongs under, so that a pairing such as a flat layout
/// under the flat category can be checked at runtime. See ValidForTypeExtension for the lookup.
/// </summary>
/// <typeparam name="T">Enum of the parent category.</typeparam>
[AttributeUsage(AttributeTargets.Field)]
public class ValidForTypeAttribute<T> : Attribute where T : Enum
{
    /// <summary>Parent category this member is valid for.</summary>
    public T Type { get; }

    public ValidForTypeAttribute(T type)
    {
        Type = type;
    }
}