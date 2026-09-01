using System.Globalization;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.Enums.Extensions;

namespace Client.Features.SRealty;

/// <summary>Turns the raw advert fields into the Czech wording the pages show.</summary>
public static class AdvertDisplay
{
    private static readonly CultureInfo Czech = CultureInfo.GetCultureInfo("cs-CZ");

    /// <summary>Headline of an advert: the function and the subtype joined by a dash.</summary>
    public static string Title(SrealityAdvertDto advert)
    {
        var function = advert.AdvertFunction?.GetLocalizedDisplayName();
        var what = advert.AdvertSubtype?.GetLocalizedDisplayName()
                   ?? advert.AdvertType?.GetLocalizedDisplayName();
        return string.Join(" – ", new[] { function, what }.Where(part => !string.IsNullOrWhiteSpace(part)));
    }

    /// <summary>Address line from street, house numbers, city part and city, whichever are filled in.</summary>
    public static string Location(SrealityAdvertDto advert)
    {
        var street = advert.LocalityStreet;
        if (!string.IsNullOrWhiteSpace(street))
        {
            var number = string.Join("/", new[] { advert.LocalityCp, advert.LocalityCo }
                .Where(n => !string.IsNullOrWhiteSpace(n)));
            if (number.Length > 0)
            {
                street = $"{street} {number}";
            }
        }

        return string.Join(", ", new[] { street, advert.LocalityCityPart, advert.LocalityCity }
            .Where(part => !string.IsNullOrWhiteSpace(part))
            .Distinct());
    }

    /// <summary>Price with its currency and unit, or the note that the price is given on request.</summary>
    public static string Price(SrealityAdvertDto advert)
    {
        if (advert.AdvertPrice is not { } price)
        {
            return "Cena na vyžádání";
        }

        var text = price.ToString("N0", Czech);
        text += " " + (advert.AdvertPriceCurrency?.GetLocalizedDisplayName() ?? "Kč");
        if (advert.AdvertPriceUnit is { } unit)
        {
            text += " " + unit.GetLocalizedDisplayName();
        }

        return text;
    }

    /// <summary>The area a card leads with; which one is filled in depends on the advert type.</summary>
    public static string? MainArea(SrealityAdvertDto advert)
    {
        var area = advert.UsableArea ?? advert.FloorArea ?? advert.EstateArea ?? advert.BuildingArea;
        return area is { } value ? $"{value.ToString("N0", Czech)} m²" : null;
    }
}
