using System.Text.Json.Serialization;

namespace Shared.SRealty.Photo.EditPhoto;

/// <summary>Metadata of an existing photo that is being changed. The image file itself is optional on that endpoint.</summary>
public sealed record EditPhotoRequest
{
    /// <summary>Key of the photo in the agency own system.</summary>
    [JsonPropertyName("photo_rkid")]
    public string? PhotoRkid { get; set; }

    [JsonPropertyName("room_type")]
    public PhotoRoomTypeEnum? RoomType { get; set; }
    
    [JsonPropertyName("photo_kind")]
    public PhotoKindEnum? PhotoKind { get; set; }
    
    /// <summary>Requested position in the gallery.</summary>
    [JsonPropertyName("order")]
    public int? Order { get; set; }
    
    /// <summary>Marks the photo that should lead the gallery.</summary>
    [JsonPropertyName("main")]
    public int? Main { get; set; }

    /// <summary>Alternative text describing the image.</summary>
    [JsonPropertyName("alt")]
    public string? Alt { get; set; }
}
