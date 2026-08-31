using ImageMagick;
using Microsoft.Extensions.Options;

namespace Server.Features.SRealty.Photo;

/// <summary>
/// Keeps the images as ordinary files on disk, one folder per advert: {RootPath}/{advertId}/{photoId}.jpg.
/// Every upload is validated, scaled down, stripped of metadata and re-encoded as JPEG, so what lands on disk
/// is never the caller's bytes.
/// </summary>
public sealed class FilePhotoStorage : IPhotoStorage
{
    private const string Extension = ".jpg";

    /// <summary>Process-wide ceiling on the pixel cache ImageMagick keeps in RAM; past it the cache spills to disk.</summary>
    private const ulong MagickMemoryLimitBytes = 512UL * 1024 * 1024;

    private static readonly MagickFormat[] AcceptedFormats =
        [MagickFormat.Jpeg, MagickFormat.Png, MagickFormat.WebP, MagickFormat.Heic, MagickFormat.Heif];

    private readonly PhotoStorageOptions _options;
    private readonly string _rootPath;

    static FilePhotoStorage()
    {
        ResourceLimits.Memory = MagickMemoryLimitBytes;
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

    /// <summary>
    /// Validates, normalizes and writes an image, returning the path it can be read back by.
    /// Throws <see cref="InvalidPhotoException"/> when the content is not an accepted image.
    /// </summary>
    public async Task<string> SaveAsync(Guid advertId, Guid photoId, Stream content, CancellationToken cancellationToken = default)
    {
        var data = await ReadUploadAsync(content, cancellationToken);

        using var image = new MagickImage();
        try
        {
            // Header only: identify the format and refuse oversized sources before a single pixel is allocated.
            image.Ping(data);
            if (!AcceptedFormats.Contains(image.Format))
            {
                throw new InvalidPhotoException("The file is not a JPEG, PNG, WebP or HEIC image.");
            }

            if (image.Width > _options.MaxSourceEdge || image.Height > _options.MaxSourceEdge)
            {
                throw new InvalidPhotoException(
                    $"The image is {image.Width}×{image.Height} px; at most {_options.MaxSourceEdge} px on a side is accepted.");
            }

            image.Read(data, new MagickReadSettings { Format = image.Format });
        }
        catch (MagickException e)
        {
            throw new InvalidPhotoException("The file could not be decoded as an image.", e);
        }

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

    /// <summary>Opens an image for reading, or returns null when nothing is stored under that path.</summary>
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

    /// <summary>Removes the image stored under the given path, and the advert folder once it is empty.</summary>
    public Task DeleteAsync(string storagePath, CancellationToken cancellationToken = default)
    {
        var fullPath = ResolveFullPath(storagePath);
        File.Delete(fullPath);

        var advertDirectory = Path.GetDirectoryName(fullPath)!;
        if (advertDirectory != _rootPath && Directory.Exists(advertDirectory) && !Directory.EnumerateFileSystemEntries(advertDirectory).Any())
        {
            Directory.Delete(advertDirectory);
        }

        return Task.CompletedTask;
    }

    /// <summary>Removes every image of an advert in one go, folder included.</summary>
    public Task DeleteAdvertAsync(Guid advertId, CancellationToken cancellationToken = default)
    {
        var advertDirectory = ResolveFullPath(advertId.ToString("D"));
        if (Directory.Exists(advertDirectory))
        {
            Directory.Delete(advertDirectory, recursive: true);
        }

        return Task.CompletedTask;
    }

    /// <summary>Buffers the upload, refusing it once it grows past the configured ceiling.</summary>
    private async Task<byte[]> ReadUploadAsync(Stream content, CancellationToken cancellationToken)
    {
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

    /// <summary>Orientation baked in, metadata gone, alpha flattened, colors in sRGB, bounded size, JPEG.</summary>
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
            image.TransformColorSpace(ColorProfiles.SRGB);
        }

        if (image.Width > _options.MaxLongEdge || image.Height > _options.MaxLongEdge)
        {
            image.Resize(new MagickGeometry(_options.MaxLongEdge, _options.MaxLongEdge));
        }

        image.Format = MagickFormat.Jpeg;
        image.Quality = _options.JpegQuality;
        image.Settings.Interlace = Interlace.Jpeg;
    }

    /// <summary>Turns a stored path into an absolute one and refuses anything that would escape the root.</summary>
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
