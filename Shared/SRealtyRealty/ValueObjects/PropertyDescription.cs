namespace Shared.SRealtyRealty.ValueObjects;

public record PropertyDescription
{
    public string Description { get; init; }

    public string? DescriptionEn { get; init; }

    public string? DescriptionRu { get; init; }

    public PropertyDescription(string description)
    {
        Description = description;
    }
}