namespace Shared.Ruian;

/// <summary>A named place of the RUIAN register: a municipality, a municipality part or a street.</summary>
public sealed record RuianPlaceDto
{
    public int Code { get; set; }

    public string? Name { get; set; }
}
