namespace Client.Features.SRealty.Components;

/// <summary>One pin on the advert map; the URL, when given, links the popup to the advert.</summary>
public sealed record AdvertMapMarker(double Lat, double Lng, string? Title, string? Url);
