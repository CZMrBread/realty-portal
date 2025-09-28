namespace Shared.SRealtyRealty.ValueObjects;

public sealed record ExclusiveValues<TValue1, TValue2>()
{
    public TValue1? Value1 { get; }
    public TValue2? Value2 { get; }

    public ExclusiveValues(TValue1? value1, TValue2? value2) : this()
    {
        Value1 = value1;
        Value2 = value2;
    }

    public bool IsValid()
    {
        return (Value1 is not null && Value2 is null) || (Value1 is null && Value2 is not null);
    }
    
}