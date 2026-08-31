using ImageMagick;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Server.Features.SRealty.Photo;

namespace Server.Tests.Features.SRealty.Photo;

public sealed class FilePhotoStorageTests : IDisposable
{
    private readonly string _root = Path.Combine(Path.GetTempPath(), "realty-portal-tests", Guid.NewGuid().ToString("N"));
    private readonly FilePhotoStorage _storage;

    public FilePhotoStorageTests()
    {
        var options = Options.Create(new PhotoStorageOptions { RootPath = _root, MaxLongEdge = 1920, JpegQuality = 85 });
        _storage = new FilePhotoStorage(options, new StubEnvironment());
    }

    public void Dispose()
    {
        if (Directory.Exists(_root))
        {
            Directory.Delete(_root, recursive: true);
        }
    }

    [Fact]
    public void MagickNet_CanDecodeEveryAcceptedFormatOnThisPlatform()
    {
        // HEIC in particular relies on the libheif that Magick.NET bundles; if a platform build lacks it, iPhone uploads
        // would pass the signature check and then fail to decode.
        foreach (var format in new[] { MagickFormat.Jpeg, MagickFormat.Png, MagickFormat.WebP, MagickFormat.Heic })
        {
            var info = MagickFormatInfo.Create(format);
            Assert.True(info?.SupportsReading, $"{format} is not readable by this Magick.NET build.");
        }
    }

    // --- SaveAsync ---

    [Fact]
    public async Task SaveAsync_ConvertsPngWithAlphaToJpegUnderAdvertFolder()
    {
        var advertId = Guid.NewGuid();
        var photoId = Guid.NewGuid();
        using var png = MakeImage(400, 300, MagickFormat.Png, withAlpha: true);

        var path = await _storage.SaveAsync(advertId, photoId, png);

        Assert.Equal($"{advertId:D}/{photoId:D}.jpg", path);
        var fullPath = Path.Combine(_root, advertId.ToString("D"), $"{photoId:D}.jpg");
        Assert.True(File.Exists(fullPath));

        using var stored = new MagickImage(fullPath);
        Assert.Equal(MagickFormat.Jpeg, stored.Format);
        Assert.False(stored.HasAlpha);
        Assert.Equal(400u, stored.Width);
        Assert.Equal(300u, stored.Height);
        Assert.Empty(Directory.GetFiles(Path.GetDirectoryName(fullPath)!, "*.tmp"));
    }

    [Fact]
    public async Task SaveAsync_DownscalesToMaxLongEdgeKeepingAspectRatio()
    {
        using var big = MakeImage(4000, 2000, MagickFormat.Jpeg);

        var path = await _storage.SaveAsync(Guid.NewGuid(), Guid.NewGuid(), big);

        using var stored = new MagickImage(Path.Combine(_root, path));
        Assert.Equal(1920u, stored.Width);
        Assert.Equal(960u, stored.Height);
    }

    [Fact]
    public async Task SaveAsync_StripsMetadata()
    {
        using var image = new MagickImage(MagickColors.Red, 32, 32);
        var exif = new ExifProfile();
        exif.SetValue(ExifTag.Artist, "leaks");
        image.SetProfile(exif);
        using var stream = new MemoryStream();
        image.Write(stream, MagickFormat.Jpeg);
        stream.Position = 0;

        var path = await _storage.SaveAsync(Guid.NewGuid(), Guid.NewGuid(), stream);

        using var stored = new MagickImage(Path.Combine(_root, path));
        Assert.Null(stored.GetExifProfile());
    }

    [Fact]
    public async Task SaveAsync_RejectsNonImage()
    {
        using var text = new MemoryStream("definitely not an image"u8.ToArray());

        await Assert.ThrowsAsync<InvalidPhotoException>(() => _storage.SaveAsync(Guid.NewGuid(), Guid.NewGuid(), text));
        Assert.Empty(Directory.EnumerateFileSystemEntries(_root));
    }

    [Fact]
    public async Task SaveAsync_RejectsDecodableButUnacceptedFormats()
    {
        using var gif = MakeImage(16, 16, MagickFormat.Gif);
        using var svg = new MemoryStream("<svg xmlns='http://www.w3.org/2000/svg' width='4' height='4'/>"u8.ToArray());

        await Assert.ThrowsAsync<InvalidPhotoException>(() => _storage.SaveAsync(Guid.NewGuid(), Guid.NewGuid(), gif));
        await Assert.ThrowsAsync<InvalidPhotoException>(() => _storage.SaveAsync(Guid.NewGuid(), Guid.NewGuid(), svg));
    }

    [Fact]
    public async Task SaveAsync_RejectsJpegSignatureWithGarbageBody()
    {
        var bytes = new byte[] { 0xFF, 0xD8, 0xFF }.Concat(new byte[64]).ToArray();
        using var stream = new MemoryStream(bytes);

        await Assert.ThrowsAsync<InvalidPhotoException>(() => _storage.SaveAsync(Guid.NewGuid(), Guid.NewGuid(), stream));
    }

    [Fact]
    public async Task SaveAsync_RejectsOversizedSourceBeforeDecoding()
    {
        var options = Options.Create(new PhotoStorageOptions { RootPath = _root, MaxSourceEdge = 1024 });
        var storage = new FilePhotoStorage(options, new StubEnvironment());
        using var wide = MakeImage(2048, 16, MagickFormat.Png);

        var e = await Assert.ThrowsAsync<InvalidPhotoException>(() => storage.SaveAsync(Guid.NewGuid(), Guid.NewGuid(), wide));
        Assert.Contains("2048", e.Message);
    }

    [Fact]
    public async Task SaveAsync_RejectsUploadOverByteLimit()
    {
        var options = Options.Create(new PhotoStorageOptions { RootPath = _root, MaxUploadBytes = 2048 });
        var storage = new FilePhotoStorage(options, new StubEnvironment());
        using var image = MakeImage(256, 256, MagickFormat.Png, noisy: true);
        Assert.True(image.Length > 2048);

        await Assert.ThrowsAsync<InvalidPhotoException>(() => storage.SaveAsync(Guid.NewGuid(), Guid.NewGuid(), image));
    }

    // --- OpenReadAsync / DeleteAsync ---

    [Fact]
    public async Task OpenReadAsync_ReturnsNullForMissingAndRefusesEscape()
    {
        Assert.Null(await _storage.OpenReadAsync($"{Guid.NewGuid():D}/{Guid.NewGuid():D}.jpg"));
        await Assert.ThrowsAsync<ArgumentException>(() => _storage.OpenReadAsync("../../etc/passwd"));
        await Assert.ThrowsAsync<ArgumentException>(() => _storage.OpenReadAsync("/etc/passwd"));
    }

    [Fact]
    public async Task DeleteAsync_RemovesFileAndEmptyAdvertFolder()
    {
        var advertId = Guid.NewGuid();
        using var a = MakeImage(32, 32, MagickFormat.Jpeg);
        using var b = MakeImage(32, 32, MagickFormat.Jpeg);
        var pathA = await _storage.SaveAsync(advertId, Guid.NewGuid(), a);
        var pathB = await _storage.SaveAsync(advertId, Guid.NewGuid(), b);
        var advertDirectory = Path.Combine(_root, advertId.ToString("D"));

        await _storage.DeleteAsync(pathA);
        Assert.True(Directory.Exists(advertDirectory));

        await _storage.DeleteAsync(pathB);
        Assert.False(Directory.Exists(advertDirectory));
        Assert.True(Directory.Exists(_root));
    }

    [Fact]
    public async Task DeleteAdvertAsync_RemovesWholeFolder()
    {
        var advertId = Guid.NewGuid();
        using var a = MakeImage(32, 32, MagickFormat.Jpeg);
        await _storage.SaveAsync(advertId, Guid.NewGuid(), a);

        await _storage.DeleteAdvertAsync(advertId);

        Assert.False(Directory.Exists(Path.Combine(_root, advertId.ToString("D"))));
        await _storage.DeleteAdvertAsync(advertId); // idempotent
    }

    // --- Helpers ---

    private static MemoryStream MakeImage(uint width, uint height, MagickFormat format, bool withAlpha = false, bool noisy = false)
    {
        using var image = new MagickImage(withAlpha ? MagickColors.Transparent : MagickColors.SteelBlue, width, height);
        if (withAlpha)
        {
            image.HasAlpha = true;
        }

        if (noisy)
        {
            // Solid colours compress to almost nothing; noise gives the file real bulk.
            image.AddNoise(NoiseType.Random);
        }

        var stream = new MemoryStream();
        image.Write(stream, format);
        stream.Position = 0;
        return stream;
    }

    private sealed class StubEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = "Testing";
        public string ApplicationName { get; set; } = "Server.Tests";
        public string ContentRootPath { get; set; } = Path.GetTempPath();
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
