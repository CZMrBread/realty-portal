namespace Server.Features.SRealty.Photo;

/// <summary>Keeps the images as ordinary files on disk.</summary>
public sealed class FilePhotoStorage:IPhotoStorage
{
    public Task<string> SaveAsync(Guid advertId, Guid photoId, Stream content, string contentType)
        => throw new NotImplementedException();

    public Task<Stream?> OpenReadAsync(string storagePath)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string storagePath)
    {
        throw new NotImplementedException();
    }
}