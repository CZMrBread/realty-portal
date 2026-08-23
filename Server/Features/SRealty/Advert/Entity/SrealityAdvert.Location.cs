using Shared.SRealty.Advert.Enums;

namespace Server.Features.SRealty.Advert.Entity;

public partial class SrealityAdvertEntity
{
    public required string LocalityCity { get; set; }
    public required int LocalityInaccuracyLevel { get; set; }
    public string? LocalityCityPart { get; set; }
    public string? LocalityStreet { get; set; }
    public string? LocalityCp { get; set; }
    public string? LocalityCo { get; set; }
    public double? LocalityLatitude { get; set; }
    public double? LocalityLongitude { get; set; }
    public int? LocalityRuian { get; set; }
    public RuianLevelEnum? LocalityRuianLevel { get; set; }
    public int? LocalityUir { get; set; }
    public UirLevelEnum? LocalityUirLevel { get; set; }
    public ObjectLocationEnum? ObjectLocation { get; set; }
    public SurroundingsTypeEnum? SurroundingsType { get; set; }
    public ProtectionEnum? Protection { get; set; }
    public List<RoadTypeEnum>? RoadType { get; set; }
    public List<TransportTypeEnum>? Transport { get; set; }
}
