using System.Text.Json.Serialization;

namespace Shared.SRealty.Advert;

public sealed partial record SrealityAdvertDto
{
    /// <summary>Date from which the property can be handed over.</summary>
    [JsonPropertyName("ready_date")]
    public DateOnly? ReadyDate { get; set; }

    [JsonPropertyName("sale_date")]
    public DateOnly? SaleDate { get; set; }

    /// <summary>Start of the first viewing open to interested buyers or tenants.</summary>
    [JsonPropertyName("first_tour_date")]
    public DateTimeOffset? FirstTourDate { get; set; }

    /// <summary>End of that first viewing.</summary>
    [JsonPropertyName("first_tour_date_to")]
    public DateTimeOffset? FirstTourDateTo { get; set; }
}
