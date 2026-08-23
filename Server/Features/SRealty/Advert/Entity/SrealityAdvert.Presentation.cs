namespace Server.Features.SRealty.Advert.Entity;

public partial class SrealityAdvertEntity
{
    public required string Description { get; set; }
    public string? DescriptionEn { get; set; }
    public string? DescriptionRu { get; set; }
    public List<string>? Keywords { get; set; }
    public int? Panorama { get; set; }
    public string? MapyPanoramaUrl { get; set; }
    public string? MatterportUrl { get; set; }
}
