using System.Text.Json.Serialization;

namespace Shared.SRealty.Photo.UploadPhoto;

/// <summary>Metadata sent alongside the image file when a photo is added to an advert.</summary>
public sealed record UploadPhotoRequest
{
    /// <summary>Key of the photo in the agency own system, so that a repeated import can recognise the same image.</summary>
    [JsonPropertyName("photo_rkid")]
    public string? PhotoRkid { get; set; }

    [JsonPropertyName("room_type")]
    public PhotoRoomTypeEnum? RoomType { get; set; }
    
    [JsonPropertyName("photo_kind")]
    public PhotoKindEnum? PhotoKind { get; set; }
    
    /// <summary>Requested position in the gallery. The server has the final say on the order.</summary>
    [JsonPropertyName("order")]
    public int? Order { get; set; }
    
    /// <summary>Marks the photo that should lead the gallery.</summary>
    [JsonPropertyName("main")]
    public int? Main { get; set; }

    /// <summary>Alternative text describing the image.</summary>
    [JsonPropertyName("alt")]
    public string? Alt { get; set; }

}