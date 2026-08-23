using Server.Features.RealtyAgency;
using Server.Features.RealtyAgency.Entity;
using Server.Features.RealtyAgent;
using Server.Features.RealtyAgent.Entity;
using Server.Features.SRealty.Photo;
using Server.Features.SRealty.Photo.Entity;
using Server.Infrastructure.Database;

namespace Server.Features.SRealty.Advert.Entity;

/// <summary>
/// An advert in the Sreality shape, stored in a table of its own that mirrors SrealityAdvertDto field for field.
/// It is split into partial files that follow the layout of the DTO; this one holds only the fields the portal
/// itself maintains, which no agency fills in, together with the navigation properties.
/// Everything that comes from the outside is nullable, because null is how the model records that the agency
/// sent no value at all. Collections are List rather than an interface so that Npgsql maps them onto
/// PostgreSQL arrays such as integer[].
/// </summary>
public partial class SrealityAdvertEntity: ITimeStampedEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    /// <summary>Agency the advert is published under. Null while its agent belongs to no agency.</summary>
    public Guid? RealtyAgencyId { get; set; }
    public RealtyAgencyEntity? Agency { get; set; }

    /// <summary>Foreign key of the agent inside the portal. Null when the agency identifies the agent only by SellerRkId.</summary>
    public Guid? SellerId { get; set; }
    public RealtyAgentEntity? Seller { get; set; }
    
    /// <summary>Worked out from AdvertLifetime when the advert is taken in.</summary>
    public required DateTimeOffset ExpiresAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public List<SrealityAdvertPhoto> Photos { get; set; } = [];
}
