namespace Server.Tests;

/// <summary>Values of TEnum for exercising range/definedness checks (e.g. EnumValueAttribute), reusable across
/// any enum in the codebase.</summary>
public static class EnumTestData<TEnum> where TEnum : struct, Enum
{
    /// <summary>Every value the enum declares.</summary>
    public static IEnumerable<TEnum> Valid() => Enum.GetValues<TEnum>();

    public static TheoryData<TEnum> ValidData()
    {
        var data = new TheoryData<TEnum>();
        foreach (var value in Valid())
        {
            data.Add(value);
        }
    
        return data;
    }

    /// <summary>
    /// Every integer from one below the lowest declared member to one above the highest, excluding values that
    /// are themselves declared members - sweeps every gap and both boundaries in one pass.
    /// </summary>
    public static IEnumerable<TEnum> OutOfRange()
    {
        var defined = Enum.GetValues<TEnum>();
        var min = Convert.ToInt32(defined.Min());
        var max = Convert.ToInt32(defined.Max());
        for (var raw = min - 1; raw <= max + 1; raw++)
        {
            var candidate = (TEnum)Enum.ToObject(typeof(TEnum), raw);
            if (!defined.Contains(candidate))
            {
                yield return candidate;
            }
        }
    }
    public static TheoryData<TEnum> OutOfRangeData()
    {
        var data = new TheoryData<TEnum>();
        foreach (var i in OutOfRange())
            data.Add((TEnum)Enum.ToObject(typeof(TEnum), i));
        return data;
    }
}
