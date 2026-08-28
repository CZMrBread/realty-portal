namespace Server.Features.SRealty.Photo;

/// <summary>
/// Where the image files themselves are kept. The database holds only the path this store hands back,
/// so the storage can be swapped without the advert model knowing.
/// </summary>
public interface IPhotoStorage
{
    /// <summary>Writes an image and returns the path it can be read back by.</summary>
    Task<string> SaveAsync(Guid advertId, Guid photoId, Stream content, string contentType);

    /// <summary>Opens an image for reading, or returns null when nothing is stored under that path.</summary>
    Task<Stream?> OpenReadAsync(string storagePath);

    /// <summary>Removes the image stored under the given path.</summary>
    Task DeleteAsync(string storagePath);
    
    /// <summary>
    /// Checks if the given stream is a valid image file. Returns true if it is, false otherwise.
    /// </summary>
    /// <param name="stream">The stream to validate.</param>
    /// <returns>True if the stream is a valid image file, false otherwise.</returns>
    Task<bool> ValidateAsync(Stream stream);
}
