using System.Text.Json.Serialization;

namespace Shared.SRealty.Photo.GetPhotos;

/// <summary>Advert photo metadata plus the address the image is served from.</summary>
public sealed record PhotoDto
{
    /// <summary>Identifier the portal knows the photo under.</summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>Address the image bytes are served from.</summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>Position in the gallery, where zero is the leading photo.</summary>
    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("room_type")]
    public PhotoRoomTypeEnum? RoomType { get; set; }

    [JsonPropertyName("photo_kind")]
    public PhotoKindEnum? PhotoKind { get; set; }

    /// <summary>Alternative text describing the image.</summary>
    [JsonPropertyName("alt")]
    public string? Alt { get; set; }
}
