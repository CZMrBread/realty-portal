using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.SRealty;

public sealed partial record SrealityAdvertDto
{
    [Required]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("description_en")]
    public string? DescriptionEn { get; set; }

    [JsonPropertyName("description_ru")]
    public string? DescriptionRu { get; set; }

    [JsonPropertyName("keywords")]
    public ICollection<string>? Keywords { get; set; }

    [JsonPropertyName("panorama")]
    public int? Panorama { get; set; }

    [Url]
    [JsonPropertyName("mapy_panorama_url")]
    public string? MapyPanoramaUrl { get; set; }

    [Url]
    [JsonPropertyName("matterport_url")]
    public string? MatterportUrl { get; set; }
}
