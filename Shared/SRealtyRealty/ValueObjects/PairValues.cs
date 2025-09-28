namespace Shared.SRealtyRealty.ValueObjects;

public sealed record PairValues<T>() where T : class
{
    public T? Value1 { get; }
    public T? Value2 { get; }

    public PairValues(T? value1, T? value2) : this()
    {
        Value1 = value1;
        Value2 = value2;
    }

    public bool IsValid()
    {
        return (Value1 is null && Value2 is null) || (Value1 is not null && Value2 is not null);
    }
}