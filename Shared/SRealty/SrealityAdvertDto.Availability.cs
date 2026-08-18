using System.Text.Json.Serialization;

namespace Shared.SRealty;

public sealed partial record SrealityAdvertDto
{
    [JsonPropertyName("ready_date")]
    public DateOnly? ReadyDate { get; set; }

    [JsonPropertyName("sale_date")]
    public DateOnly? SaleDate { get; set; }

    [JsonPropertyName("first_tour_date")]
    public DateTimeOffset? FirstTourDate { get; set; }

    [JsonPropertyName("first_tour_date_to")]
    public DateTimeOffset? FirstTourDateTo { get; set; }
}
