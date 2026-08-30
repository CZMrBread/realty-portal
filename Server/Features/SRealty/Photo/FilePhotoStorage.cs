using ImageMagick;
using Microsoft.Extensions.Options;

namespace Server.Features.SRealty.Photo;

/// <summary>
/// Keeps the images as ordinary files on disk, one folder per advert: {RootPath}/{advertId}/{photoId}.jpg.
/// Every upload is decoded with the format pinned to what the file signature says, scaled down, stripped of
/// metadata and re-encoded as JPEG, so what lands on disk is never the caller's bytes.
/// </summary>
public sealed class FilePhotoStorage : IPhotoStorage
{
    /// <summary>Longest prefix any recognised signature needs: the RIFF/ftyp headers read up to byte 12.</summary>
    private const int SignatureLength = 12;

    private const string Extension = ".jpg";

    // Signatures are byte arrays, not u8 literals: a u8 literal UTF-8-encodes anything above 0x7F into two bytes.
    private static ReadOnlySpan<byte> JpegSignature => [0xFF, 0xD8, 0xFF];
    private static ReadOnlySpan<byte> PngSignature => [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];

    /// <summary>HEIF brands that hold a still image; the four bytes after "ftyp" in the file header.</summary>
    private static readonly string[] HeicBrands = ["heic", "heix", "heif", "hevc", "hevx", "mif1", "msf1"];

    private readonly PhotoStorageOptions _options;
    private readonly string _rootPath;

    static FilePhotoStorage()
    {
        // Process-wide ceiling on the pixel cache ImageMagick keeps in RAM; past it the cache spills to disk instead
        // of growing. The per-upload dimension check in SaveAsync is what actually refuses oversized sources.
        ResourceLimits.Memory = 512UL * 1024 * 1024;
    }

    public FilePhotoStorage(IOptions<PhotoStorageOptions> options, IHostEnvironment environment)
    {
        _options = options.Value;
        _rootPath = Path.GetFullPath(Path.Combine(environment.ContentRootPath, _options.RootPath));
        Directory.CreateDirectory(_rootPath);
    }

    /// <summary>Path the database holds for a photo, relative to the root so the root can move.</summary>
    public static string GetRelativePath(Guid advertId, Guid photoId)
        => $"{advertId:D}/{photoId:D}{Extension}";

    public async Task<string> SaveAsync(Guid advertId, Guid photoId, Stream content, CancellationToken cancellationToken = default)
    {
        var format = await DetectFormatAsync(content, cancellationToken)
                     ?? throw new InvalidPhotoException("The file is not a JPEG, PNG, WebP or HEIC image.");

        var data = await ReadUploadAsync(content, cancellationToken);
        var readSettings = new MagickReadSettings { Format = format };

        using var image = new MagickImage();
        try
        {
            // Header only: refuse oversized sources before a single pixel is allocated.
            image.Ping(data, readSettings);
            if (image.Width > _options.MaxSourceEdge || image.Height > _options.MaxSourceEdge)
            {
                throw new InvalidPhotoException(
                    $"The image is {image.Width}×{image.Height} px; at most {_options.MaxSourceEdge} px on a side is accepted.");
            }

            image.Read(data, readSettings);
        }
        catch (MagickException e)
        {
            throw new InvalidPhotoException("The file could not be decoded as an image.", e);
        }

        cancellationToken.ThrowIfCancellationRequested();
        Normalise(image);

        var relativePath = GetRelativePath(advertId, photoId);
        var fullPath = ResolveFullPath(relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        // Write beside the target and move into place, so a reader never sees a half-written file.
        var tempPath = $"{fullPath}.{Guid.NewGuid():N}.tmp";
        try
        {
            await using (var file = new FileStream(tempPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, useAsync: true))
            {
                await image.WriteAsync(file, cancellationToken);
            }

            File.Move(tempPath, fullPath, overwrite: true);
        }
        catch
        {
            File.Delete(tempPath);
            throw;
        }

        return relativePath;
    }

    public Task<Stream?> OpenReadAsync(string storagePath, CancellationToken cancellationToken = default)
    {
        var fullPath = ResolveFullPath(storagePath);
        if (!File.Exists(fullPath))
        {
            return Task.FromResult<Stream?>(null);
        }

        Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
        return Task.FromResult<Stream?>(stream);
    }

    public Task DeleteAsync(string storagePath, CancellationToken cancellationToken = default)
    {
        var fullPath = ResolveFullPath(storagePath);
        File.Delete(fullPath);

        // Leave no empty advert folders behind.
        var advertDirectory = Path.GetDirectoryName(fullPath)!;
        if (advertDirectory != _rootPath && Directory.Exists(advertDirectory) && !Directory.EnumerateFileSystemEntries(advertDirectory).Any())
        {
            Directory.Delete(advertDirectory);
        }

        return Task.CompletedTask;
    }

    public Task DeleteAdvertAsync(Guid advertId, CancellationToken cancellationToken = default)
    {
        var advertDirectory = ResolveFullPath(advertId.ToString("D"));
        if (Directory.Exists(advertDirectory))
        {
            Directory.Delete(advertDirectory, recursive: true);
        }

        return Task.CompletedTask;
    }

    public async Task<bool> ValidateAsync(Stream stream, CancellationToken cancellationToken = default)
        => await DetectFormatAsync(stream, cancellationToken) is not null;

    // --- Helpers ---

    /// <summary>
    /// Sniffs the file signature (magic bytes) rather than trusting the client-supplied content type, which is
    /// trivially spoofed. The stream is rewound to where it started when it is seekable.
    /// </summary>
    private static async Task<MagickFormat?> DetectFormatAsync(Stream stream, CancellationToken cancellationToken)
    {
        var start = stream.CanSeek ? stream.Position : 0;
        var header = new byte[SignatureLength];
        var read = await stream.ReadAtLeastAsync(header, SignatureLength, throwOnEndOfStream: false, cancellationToken);

        if (stream.CanSeek)
        {
            stream.Position = start;
        }

        return read >= SignatureLength ? DetectFormat(header) : null;
    }

    private static MagickFormat? DetectFormat(ReadOnlySpan<byte> header)
    {
        // JPEG: FF D8 FF
        if (header[..JpegSignature.Length].SequenceEqual(JpegSignature))
        {
            return MagickFormat.Jpeg;
        }

        // PNG: 89 'P' 'N' 'G' 0D 0A 1A 0A
        if (header[..PngSignature.Length].SequenceEqual(PngSignature))
        {
            return MagickFormat.Png;
        }

        // WebP: 'R' 'I' 'F' 'F' <4-byte size> 'W' 'E' 'B' 'P'
        if (header[..4].SequenceEqual("RIFF"u8) && header[8..12].SequenceEqual("WEBP"u8))
        {
            return MagickFormat.WebP;
        }

        // HEIC/HEIF (ISO BMFF): <4-byte size> 'f' 't' 'y' 'p' <4-byte brand>
        if (header[4..8].SequenceEqual("ftyp"u8))
        {
            var brand = System.Text.Encoding.ASCII.GetString(header[8..12]);
            if (HeicBrands.Contains(brand))
            {
                return MagickFormat.Heic;
            }
        }

        return null;
    }

    /// <summary>Buffers the upload, refusing it once it grows past the configured ceiling.</summary>
    private async Task<byte[]> ReadUploadAsync(Stream content, CancellationToken cancellationToken)
    {
        if (content.CanSeek && content.Length - content.Position > _options.MaxUploadBytes)
        {
            throw new InvalidPhotoException($"The file is larger than the {_options.MaxUploadBytes / (1024 * 1024)} MB limit.");
        }

        using var buffer = new MemoryStream();
        var chunk = new byte[81920];
        int read;
        while ((read = await content.ReadAsync(chunk, cancellationToken)) > 0)
        {
            if (buffer.Length + read > _options.MaxUploadBytes)
            {
                throw new InvalidPhotoException($"The file is larger than the {_options.MaxUploadBytes / (1024 * 1024)} MB limit.");
            }

            buffer.Write(chunk, 0, read);
        }

        return buffer.ToArray();
    }

    /// <summary>Orientation baked in, metadata gone, alpha flattened, colours in sRGB, bounded size, JPEG.</summary>
    private void Normalise(MagickImage image)
    {
        image.AutoOrient();
        image.Strip();

        if (image.HasAlpha)
        {
            image.BackgroundColor = MagickColors.White;
            image.Alpha(AlphaOption.Remove);
        }

        if (image.ColorSpace != ColorSpace.sRGB)
        {
            image.TransformColorSpace(ColorProfile.SRGB);
        }

        if (image.Width > _options.MaxLongEdge || image.Height > _options.MaxLongEdge)
        {
            // MagickGeometry(w, h) keeps the aspect ratio and fits inside the box.
            image.Resize(new MagickGeometry(_options.MaxLongEdge, _options.MaxLongEdge));
        }

        image.Format = MagickFormat.Jpeg;
        image.Quality = _options.JpegQuality;
        image.Settings.Interlace = Interlace.Jpeg;
    }

    /// <summary>
    /// Turns a stored path into an absolute one and refuses anything that would escape the root, so a tampered
    /// database value like "../../etc/passwd" cannot reach outside the photo folder.
    /// </summary>
    private string ResolveFullPath(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath) || Path.IsPathRooted(relativePath))
        {
            throw new ArgumentException("Storage path must be relative to the photo root.", nameof(relativePath));
        }

        var fullPath = Path.GetFullPath(Path.Combine(_rootPath, relativePath));
        if (!fullPath.StartsWith(_rootPath + Path.DirectorySeparatorChar, StringComparison.Ordinal))
        {
            throw new ArgumentException("Storage path escapes the photo root.", nameof(relativePath));
        }

        return fullPath;
    }
}
