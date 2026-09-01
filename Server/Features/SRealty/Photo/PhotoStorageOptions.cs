using System.ComponentModel.DataAnnotations;

namespace Server.Features.SRealty.Photo;

/// <summary>Settings of <see cref="FilePhotoStorage"/>, bound from the "PhotoStorage" section.</summary>
public sealed class PhotoStorageOptions
{
    public const string SectionName = "PhotoStorage";

    /// <summary>Directory the images live under; a relative path is taken from the content root.</summary>
    [Required]
    public string RootPath { get; set; } = "data/photos";

    /// <summary>Longest side of a stored image; bigger uploads are scaled down.</summary>
    [Range(256, 8192)]
    public uint MaxLongEdge { get; set; } = 1920;

    /// <summary>JPEG quality of the stored image, 1 to 100.</summary>
    [Range(1, 100)]
    public uint JpegQuality { get; set; } = 85;

    /// <summary>Longest declared side accepted before decoding; guards against decompression bombs.</summary>
    [Range(1024, 65535)]
    public uint MaxSourceEdge { get; set; } = 12_000;

    /// <summary>Upper bound on the raw upload, in bytes.</summary>
    [Range(1024, long.MaxValue)]
    public long MaxUploadBytes { get; set; } = 25 * 1024 * 1024;
}
