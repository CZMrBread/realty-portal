using Microsoft.EntityFrameworkCore;
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
    public async Task<List<SrealityAdvertPhotoEntity>> GetAdvertPhotosAsync(Guid advertId)
    {
        return await appDbContext.SrealityAdvertPhotos.Where(a => a.SrealityAdvertId == advertId).ToListAsync();
    }

    /// <summary>Photo with the given identifier, or null when there is none.</summary>
    public async Task<SrealityAdvertPhotoEntity?> FindPhotoByIdAsync(Guid photoId)
    {
        return await appDbContext.SrealityAdvertPhotos.FirstOrDefaultAsync(a => a.SrealityAdvertId == photoId);
    }

    /// <summary>Photo with the given identifier, provided it really belongs to the given advert.</summary>
    public async Task<SrealityAdvertPhotoEntity?> FindPhotoByAdvertIdAndIdAsync(Guid advertId, Guid photoId)
    {
        return await appDbContext.SrealityAdvertPhotos.Where(a => a.Id == photoId  && a.SrealityAdvertId == advertId).FirstOrDefaultAsync();
    }

    // --- Create / Update / Delete ---

    /// <summary>Files the image in the store and records its metadata against the advert.</summary>
    public Task<SrealityAdvertPhotoEntity> AddPhotoAsync(SrealityAdvertEntity advert, Stream content,
        string contentType,
        UploadPhotoRequest request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    /// <summary>Replaces the image of an existing photo and updates its metadata.</summary>
    public Task<SrealityAdvertPhotoEntity> UpdatePhotoAsync(SrealityAdvertPhotoEntity photoEntity, Stream content, string contentType,
        UploadPhotoRequest request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    /// <summary>Puts the gallery into the given order and returns the photos as they now stand.</summary>
    public Task<List<SrealityAdvertPhotoEntity>> ReorderPhotosAsync(Guid advertId, List<Guid> orderedPhotoIds)
        => throw new NotImplementedException();

    /// <summary>Removes one photo together with its image file.</summary>
    public Task DeletePhotoAsync(SrealityAdvertPhotoEntity photoEntity)
        => throw new NotImplementedException();

    /// <summary>Removes every photo of an advert together with the image files.</summary>
    public Task DeleteAdvertPhotosAsync(Guid advertId)
        => throw new NotImplementedException();
}
