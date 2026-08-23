using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.SRealty.Advert;

public sealed partial record SrealityAdvertDto
{
    [Required]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("description_en")]
    public string? DescriptionEn { get; set; }

    [JsonPropertyName("description_ru")]
    public string? DescriptionRu { get; set; }

    /// <summary>Search keywords the agency adds to the advert.</summary>
    [JsonPropertyName("keywords")]
    public ICollection<string>? Keywords { get; set; }

    [JsonPropertyName("panorama")]
    public int? Panorama { get; set; }

    /// <summary>Link to a street-level panorama of the address.</summary>
    [Url]
    [JsonPropertyName("mapy_panorama_url")]
    public string? MapyPanoramaUrl { get; set; }

    /// <summary>Link to a Matterport walkthrough of the interior.</summary>
    [Url]
    [JsonPropertyName("matterport_url")]
    public string? MatterportUrl { get; set; }
}
