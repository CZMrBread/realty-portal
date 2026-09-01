namespace Server.Features.SRealty.Photo;

/// <summary>Store of the image files; the database holds only the path the store hands back.</summary>
public interface IPhotoStorage
{
    /// <summary>
    /// Validates, normalizes (JPEG, bounded size, metadata stripped) and writes an image, returning its stored path.
    /// Throws <see cref="InvalidPhotoException"/> when the content is not an accepted image.
    /// </summary>
    Task<string> SaveAsync(Guid advertId, Guid photoId, Stream content, CancellationToken cancellationToken = default);

    /// <summary>Opens an image for reading, or null when nothing is stored under that path.</summary>
    Task<Stream?> OpenReadAsync(string storagePath, CancellationToken cancellationToken = default);

    /// <summary>Removes the image stored under the given path.</summary>
    Task DeleteAsync(string storagePath, CancellationToken cancellationToken = default);

    /// <summary>Removes every image of an advert.</summary>
    Task DeleteAdvertAsync(Guid advertId, CancellationToken cancellationToken = default);
}
