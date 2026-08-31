using Server.Features.SRealty.Advert.Entity;
using Server.Infrastructure.Database;
using Shared.SRealty.Photo;

namespace Server.Features.SRealty.Photo.Entity;

/// <summary>
/// A photo belonging to an advert. The image itself lives in the store behind <see cref="IPhotoStorage"/>;
/// this entity holds only the metadata.
/// </summary>
public class SrealityAdvertPhotoEntity : ITimeStampedEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public Guid SrealityAdvertId { get; set; }
    public SrealityAdvertEntity Advert { get; set; } = null!;

    /// <summary>Path or key the image is filed under in the photo store.</summary>
    public required string StoragePath { get; set; }

    /// <summary>Key of the photo in the agency own system, so that a repeated import can recognise the same image.</summary>
    public string? PhotoRkId { get; set; }

    /// <summary>Position in the gallery, where zero is the leading photo.</summary>
    public int Order { get; set; }

    public PhotoRoomTypeEnum? RoomType { get; set; }

    public PhotoKindEnum? PhotoKind { get; set; }

    /// <summary>Alternative text describing the image.</summary>
    public string? Alt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
