using Shared.SRealty.Advert.Enums;

namespace Server.Features.SRealty.Advert.Entity;

public partial class SrealityAdvertEntity
{
    public OwnershipTypeEnum? Ownership { get; set; }
    public double? Personal { get; set; }
    public int? NumOwners { get; set; }
    public int? ShareNumerator { get; set; }
    public int? ShareDenominator { get; set; }
    public int? ShareCommonAreaNumerator { get; set; }
    public int? ShareCommonAreaDenominator { get; set; }
}
