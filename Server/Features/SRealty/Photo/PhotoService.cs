using Server.Features.SRealty.Advert;
using Server.Features.SRealty.Advert.Entity;
using Server.Features.SRealty.Photo.Entity;
using Server.Infrastructure.Database;
using Shared.SRealty.Photo;
using Shared.SRealty.Photo.UploadPhoto;

namespace Server.Features.SRealty.Photo;

/// <summary>
/// Keeps the photo metadata in the database and the image files in <see cref="IPhotoStorage"/> in step with
/// each other.
/// </summary>
public sealed class PhotoService(AppDbContext appDbContext, IPhotoStorage photoStorage)
{
    // --- Get ---

    /// <summary>Every photo of an advert, in gallery order.</summary>
    public Task<List<SrealityAdvertPhoto>> GetAdvertPhotosAsync(Guid advertId)
        => throw new NotImplementedException();

    /// <summary>Photo with the given identifier, or null when there is none.</summary>
    public Task<SrealityAdvertPhoto?> FindPhotoByIdAsync(Guid photoId)
        => throw new NotImplementedException();
    
    /// <summary>Photo with the given identifier, provided it really belongs to the given advert.</summary>
    public Task<SrealityAdvertPhoto> FindPhotoByAdvertIdAndIdAsync(Guid advertId, Guid photoId)
        => throw new NotImplementedException();

    // --- Create / Update / Delete ---

    /// <summary>Files the image in the store and records its metadata against the advert.</summary>
    public Task<SrealityAdvertPhoto> AddPhotoAsync(SrealityAdvertEntity advert, Stream content, string contentType,
        UploadPhotoRequest request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    /// <summary>Replaces the image of an existing photo and updates its metadata.</summary>
    public Task<SrealityAdvertPhoto> UpdatePhotoAsync(SrealityAdvertPhoto photo, Stream content, string contentType,
        UploadPhotoRequest request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    /// <summary>Puts the gallery into the given order and returns the photos as they now stand.</summary>
    public Task<List<SrealityAdvertPhoto>> ReorderPhotosAsync(Guid advertId, List<Guid> orderedPhotoIds)
        => throw new NotImplementedException();

    /// <summary>Removes one photo together with its image file.</summary>
    public Task DeletePhotoAsync(SrealityAdvertPhoto photo)
        => throw new NotImplementedException();

    /// <summary>Removes every photo of an advert together with the image files.</summary>
    public Task DeleteAdvertPhotosAsync(Guid advertId)
        => throw new NotImplementedException();
}