namespace Shared.SRealtyRealty.Enums;

[AttributeUsage(AttributeTargets.Field)]
public class ValidForTypeAttribute<T> : Attribute where T : Enum
{
    public T Type { get; }

    public ValidForTypeAttribute(T type)
    {
        Type = type;
    }
}