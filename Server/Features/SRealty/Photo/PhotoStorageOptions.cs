using System.ComponentModel.DataAnnotations;

namespace Server.Features.SRealty.Photo;

/// <summary>Where and how <see cref="FilePhotoStorage"/> keeps the images. Bound from the "PhotoStorage" section.</summary>
public sealed class PhotoStorageOptions
{
    public const string SectionName = "PhotoStorage";

    /// <summary>
    /// Directory the images live under. A relative path is taken from the content root; in production point it
    /// at an absolute path on a persistent volume. Files are filed as {RootPath}/{advertId}/{photoId}.jpg.
    /// </summary>
    [Required]
    public string RootPath { get; set; } = "data/photos";

    /// <summary>Longest side of a stored image; bigger uploads are scaled down to fit.</summary>
    [Range(256, 8192)]
    public uint MaxLongEdge { get; set; } = 1920;

    /// <summary>JPEG quality of the stored image, 1–100.</summary>
    [Range(1, 100)]
    public uint JpegQuality { get; set; } = 85;

    /// <summary>
    /// Uploads whose header declares a side longer than this are refused before decoding, which is what stops a
    /// decompression bomb (a tiny file that expands to gigabytes of pixels) from ever being allocated.
    /// </summary>
    [Range(1024, 65535)]
    public uint MaxSourceEdge { get; set; } = 12_000;

    /// <summary>Upper bound on the raw upload, in bytes. Phone HEIC/PNG files can reach 10+ MB.</summary>
    [Range(1024, long.MaxValue)]
    public long MaxUploadBytes { get; set; } = 25 * 1024 * 1024;
}
