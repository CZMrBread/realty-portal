using Microsoft.EntityFrameworkCore;
using Server.Features.SRealty.Advert.Entity;
using Server.Features.SRealty.Photo.Entity;
using Server.Infrastructure.Database;
using Shared.SRealty.Photo.EditPhoto;
using Shared.SRealty.Photo.UploadPhoto;

namespace Server.Features.SRealty.Photo;

/// <summary>Keeps photo metadata in the database and image files in <see cref="IPhotoStorage"/> in step.</summary>
public sealed class PhotoService(AppDbContext appDbContext, IPhotoStorage photoStorage)
{
    // --- Get ---

    /// <summary>Every photo of an advert, in gallery order. Untracked.</summary>
    public async Task<List<SrealityAdvertPhotoEntity>> GetAdvertPhotosAsync(Guid advertId,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.SrealityAdvertPhotos.AsNoTracking()
            .Where(p => p.SrealityAdvertId == advertId)
            .OrderBy(p => p.Order)
            .ToListAsync(cancellationToken);
    }

    /// <summary>Photo with the given identifier, or null when there is none. Tracked.</summary>
    public async Task<SrealityAdvertPhotoEntity?> FindPhotoByIdAsync(Guid photoId,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.SrealityAdvertPhotos
            .FirstOrDefaultAsync(p => p.Id == photoId, cancellationToken);
    }

    /// <summary>Photo with the given identifier on the given advert, or null. Tracked.</summary>
    public async Task<SrealityAdvertPhotoEntity?> FindPhotoByAdvertIdAndIdAsync(Guid advertId, Guid photoId,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.SrealityAdvertPhotos
            .FirstOrDefaultAsync(p => p.Id == photoId && p.SrealityAdvertId == advertId, cancellationToken);
    }

    /// <summary>Photo of the advert the agency knows under the given key, or null. Tracked.</summary>
    public async Task<SrealityAdvertPhotoEntity?> FindPhotoByRkIdAsync(Guid advertId, string photoRkId,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.SrealityAdvertPhotos
            .FirstOrDefaultAsync(p => p.SrealityAdvertId == advertId && p.PhotoRkId == photoRkId,
                cancellationToken);
    }

    // --- Create / Update / Delete ---

    /// <summary>
    /// Files the image in the store and appends its metadata to the gallery; an agency key the advert already
    /// carries replaces that photo instead.
    /// </summary>
    public async Task<SrealityAdvertPhotoEntity> AddPhotoAsync(SrealityAdvertEntity advert, Stream content,
        UploadPhotoRequest request, string? photoRkId = null, CancellationToken cancellationToken = default)
    {
        var rkId = photoRkId ?? request.PhotoRkid;
        if (rkId is not null && await FindPhotoByRkIdAsync(advert.Id, rkId, cancellationToken) is { } existing)
        {
            existing.StoragePath = await photoStorage.SaveAsync(advert.Id, existing.Id, content, cancellationToken);
            existing.RoomType = request.RoomType;
            existing.PhotoKind = request.PhotoKind;
            existing.Alt = request.Alt;
            await appDbContext.SaveChangesAsync(cancellationToken);
            return existing;
        }

        // MAX over an empty gallery is NULL in SQL, hence the nullable cast; the first photo then gets order 0.
        var lastOrder = await appDbContext.SrealityAdvertPhotos
            .Where(p => p.SrealityAdvertId == advert.Id)
            .MaxAsync(p => (int?)p.Order, cancellationToken) ?? -1;

        var photo = new SrealityAdvertPhotoEntity
        {
            SrealityAdvertId = advert.Id,
            PhotoRkId = rkId,
            StoragePath = string.Empty,
            Order = lastOrder + 1,
            RoomType = request.RoomType,
            PhotoKind = request.PhotoKind,
            Alt = request.Alt,
        };

        photo.StoragePath = await photoStorage.SaveAsync(advert.Id, photo.Id, content, cancellationToken);
        try
        {
            appDbContext.SrealityAdvertPhotos.Add(photo);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await photoStorage.DeleteAsync(photo.StoragePath, CancellationToken.None);
            throw;
        }

        return photo;
    }

    /// <summary>Updates photo metadata, and the image when a stream is sent; an omitted agency key is kept.</summary>
    public async Task<SrealityAdvertPhotoEntity> UpdatePhotoAsync(SrealityAdvertPhotoEntity photo, Stream? content,
        EditPhotoRequest request, CancellationToken cancellationToken = default)
    {
        if (content is not null)
        {
            photo.StoragePath = await photoStorage.SaveAsync(photo.SrealityAdvertId, photo.Id, content, cancellationToken);
        }

        if (request.PhotoRkid is not null)
        {
            photo.PhotoRkId = request.PhotoRkid;
        }

        photo.RoomType = request.RoomType;
        photo.PhotoKind = request.PhotoKind;
        photo.Alt = request.Alt;
        await appDbContext.SaveChangesAsync(cancellationToken);
        return photo;
    }

    /// <summary>Reorders the gallery and returns the photos in their new order.</summary>
    public async Task<List<SrealityAdvertPhotoEntity>> ReorderPhotosAsync(Guid advertId, List<Guid> orderedPhotoIds,
        CancellationToken cancellationToken = default)
    {
        var photos = await appDbContext.SrealityAdvertPhotos
            .Where(p => p.SrealityAdvertId == advertId)
            .ToListAsync(cancellationToken);

        // Named photos take the front in the requested order; the rest keep their relative order behind them.
        var position = orderedPhotoIds.Select((id, index) => (id, index)).ToDictionary(x => x.id, x => x.index);
        var reordered = photos
            .OrderBy(p => position.TryGetValue(p.Id, out var index) ? index : int.MaxValue)
            .ThenBy(p => p.Order)
            .ToList();
        for (var i = 0; i < reordered.Count; i++)
        {
            reordered[i].Order = i;
        }

        await appDbContext.SaveChangesAsync(cancellationToken);
        return reordered;
    }

    /// <summary>Removes a photo and its image file, closing the gap in the gallery order.</summary>
    public async Task DeletePhotoAsync(SrealityAdvertPhotoEntity photo, CancellationToken cancellationToken = default)
    {
        appDbContext.SrealityAdvertPhotos.Remove(photo);
        await appDbContext.SaveChangesAsync(cancellationToken);

        await appDbContext.SrealityAdvertPhotos
            .Where(p => p.SrealityAdvertId == photo.SrealityAdvertId && p.Order > photo.Order)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.Order, p => p.Order - 1), cancellationToken);

        await photoStorage.DeleteAsync(photo.StoragePath, cancellationToken);
    }

    /// <summary>Removes every photo of an advert, image files included.</summary>
    public async Task DeleteAdvertPhotosAsync(Guid advertId, CancellationToken cancellationToken = default)
    {
        await appDbContext.SrealityAdvertPhotos
            .Where(p => p.SrealityAdvertId == advertId)
            .ExecuteDeleteAsync(cancellationToken);
        await photoStorage.DeleteAdvertAsync(advertId, cancellationToken);
    }
}
