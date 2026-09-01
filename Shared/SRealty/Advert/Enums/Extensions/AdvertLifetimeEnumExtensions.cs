namespace Shared.SRealty.Advert.Enums.Extensions;

/// <summary>Turns the lifetime chosen for an advert into a concrete date.</summary>
public static class AdvertLifetimeEnumExtensions
{
    /// <summary>Moment an advert with this lifetime expires, counted from <paramref name="from"/>.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Undefined lifetime member.</exception>
    public static DateTimeOffset ToExpiration(this AdvertLifetimeEnum lifetime, DateTimeOffset from)
    {
        return from.AddDays(lifetime switch
        {
            AdvertLifetimeEnum.SevenDays => 7,
            AdvertLifetimeEnum.FourteenDays => 14,
            AdvertLifetimeEnum.ThirtyDays => 30,
            AdvertLifetimeEnum.FortyFiveDays => 45,
            AdvertLifetimeEnum.NinetyDays => 90,
            AdvertLifetimeEnum.OneHundredEightyDays => 180,
            AdvertLifetimeEnum.ThreeHundredSixtyDays => 360,
            _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
        });
    }
}