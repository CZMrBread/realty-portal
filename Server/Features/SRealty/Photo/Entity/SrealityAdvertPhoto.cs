using Server.Features.SRealty.Advert.Entity;
using Server.Infrastructure.Database;

namespace Server.Features.SRealty.Photo.Entity;

/// <summary>
/// A photo belonging to an advert. The image itself lives in the store behind <see cref="IPhotoStorage"/>;
/// this entity holds only the metadata.
/// </summary>
public class SrealityAdvertPhoto : ITimeStampedEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public Guid SrealityAdvertId { get; set; }
    public SrealityAdvertEntity Advert { get; set; } = null!;

    /// <summary>Path or key the image is filed under in the photo store.</summary>
    public required string StoragePath { get; set; }

    /// <summary>Position in the gallery, where zero is the leading photo.</summary>
    public int Order { get; set; }

    /// <summary>
    /// The room_type value from table 2 of the specification, the one for the addPhoto method.
    /// That table runs on to the next page, so the field stays a plain int until it has been transcribed.
    /// </summary>
    public int? RoomType { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
