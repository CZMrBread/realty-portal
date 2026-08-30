namespace Server.Features.SRealty.Photo;

/// <summary>
/// Where the image files themselves are kept. The database holds only the path this store hands back,
/// so the storage can be swapped without the advert model knowing.
/// </summary>
public interface IPhotoStorage
{
    /// <summary>
    /// Validates, normalises (JPEG, bounded size, metadata stripped) and writes an image, returning the path it can be read back by.
    /// Throws <see cref="InvalidPhotoException"/> when the content is not an accepted image.
    /// </summary>
    Task<string> SaveAsync(Guid advertId, Guid photoId, Stream content, CancellationToken cancellationToken = default);

    /// <summary>Opens an image for reading, or returns null when nothing is stored under that path.</summary>
    Task<Stream?> OpenReadAsync(string storagePath, CancellationToken cancellationToken = default);

    /// <summary>Removes the image stored under the given path.</summary>
    Task DeleteAsync(string storagePath, CancellationToken cancellationToken = default);

    /// <summary>Removes every image of an advert in one go, folder included.</summary>
    Task DeleteAdvertAsync(Guid advertId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cheap check whether the stream starts like an accepted image (JPEG, PNG, WebP, HEIC). A stream that passes is
    /// not yet proven decodable; <see cref="SaveAsync"/> is the authoritative check.
    /// </summary>
    Task<bool> ValidateAsync(Stream stream, CancellationToken cancellationToken = default);
}
