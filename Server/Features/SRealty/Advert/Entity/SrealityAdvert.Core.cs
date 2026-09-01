using Shared.SRealty.Advert.Enums;

namespace Server.Features.SRealty.Advert.Entity;

public partial class SrealityAdvertEntity
{
    /// <summary>Key of the advert in the agency's own system; unique within one agency only.</summary>
    public string? AdvertRkId { get; set; }

    /// <summary>Key of the selling agent in the agency's own system, for agents not registered in the portal.</summary>
    public string? SellerRkId { get; set; }

    public string? AdvertCode { get; set; }

    public required AdvertFunctionEnum AdvertFunction { get; set; }
    public required AdvertLifetimeEnum AdvertLifetime { get; set; }
    public required AdvertTypeEnum AdvertType { get; set; }
    public required AdvertSubtypeEnum AdvertSubtype { get; set; }
    public AdvertRoomCountEnum? AdvertRoomCount { get; set; }
    public ExtraInfoEnum? ExtraInfo { get; set; }
    public bool UserStatus { get; set; } = false;
    public bool ExclusivelyAtRk { get; set; } = false;
}
