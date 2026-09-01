using Server.Features.SRealty.Advert.Entity;
using Server.Infrastructure.Database;
using Shared.SRealty.Photo;

namespace Server.Features.SRealty.Photo.Entity;

/// <summary>Metadata of an advert photo; the image itself lives behind <see cref="IPhotoStorage"/>.</summary>
public class SrealityAdvertPhotoEntity : ITimeStampedEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public Guid SrealityAdvertId { get; set; }
    public SrealityAdvertEntity Advert { get; set; } = null!;

    /// <summary>Path or key the image is filed under in the photo store.</summary>
    public required string StoragePath { get; set; }

    /// <summary>Key of the photo in the agency's own system.</summary>
    public string? PhotoRkId { get; set; }

    /// <summary>Zero-based position in the gallery.</summary>
    public int Order { get; set; }

    public PhotoRoomTypeEnum? RoomType { get; set; }

    public PhotoKindEnum? PhotoKind { get; set; }

    /// <summary>Alternative text describing the image.</summary>
    public string? Alt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
