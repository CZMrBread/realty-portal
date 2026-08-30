using NpgsqlTypes;
using Server.Features.RealtyAgency;
using Server.Features.RealtyAgency.Entity;
using Server.Features.RealtyAgent;
using Server.Features.RealtyAgent.Entity;
using Server.Features.Ruian.Entity;
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

    /// <summary>
    /// Municipality of the RUIAN register the portal placed the advert in: from LocalityRuian when that code
    /// names a municipality, from LocalityCity otherwise. Null when neither could be matched. This is what the
    /// listing filters by district and region on; the agency-supplied address stays untouched in the Location part.
    /// </summary>
    public int? LocalityMunicipalityCode { get; set; }
    public RuianMunicipalityEntity? LocalityMunicipality { get; set; }

    /// <summary>District of <see cref="LocalityMunicipality"/>, or the district itself when the agency sent a district-level code. Set whenever the municipality is, and sometimes when it is not.</summary>
    public int? LocalityDistrictCode { get; set; }
    public RuianDistrictEntity? LocalityDistrict { get; set; }

    /// <summary>
    /// Full-text search words of the description and the address, kept up to date by PostgreSQL itself as a
    /// generated column. Never written from code, and absent from the model on any other database provider.
    /// </summary>
    public NpgsqlTsVector? SearchVector { get; set; }

    public List<SrealityAdvertPhotoEntity> Photos { get; set; } = [];

    /// <summary>
    /// Whether the given agent may change this advert: either it was published under their agency, or they are
    /// the agent named on it as the seller. An agent belonging to no agency owns only what they sell themselves,
    /// which is why two absent agencies are not a match.
    /// </summary>
    public bool IsOwnedBy(RealtyAgentEntity agent)
        => (RealtyAgencyId is not null && RealtyAgencyId == agent.RealtyAgencyId)
           || (SellerId is not null && SellerId == agent.UserId);
}
