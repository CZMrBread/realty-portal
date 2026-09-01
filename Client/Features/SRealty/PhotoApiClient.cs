using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Client.Infrastructure;
using Shared.SRealty.Photo.GetPhotos;
using Shared.SRealty.Photo.UploadPhoto;

namespace Client.Features.SRealty;

/// <summary>Talks to the photo endpoints under /srealty/advert.</summary>
public sealed class PhotoApiClient(HttpClient httpClient)
{
    /// <summary>Reads the photos of an advert in gallery order, each with its image URL.</summary>
    public async Task<(List<PhotoDto>? Response, string? Error)> GetPhotosAsync(Guid advertId,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync($"{AdvertApiClient.AdvertPath}/{advertId}/photo", cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<List<PhotoDto>>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    /// <summary>Uploads one image with its metadata to an advert as multipart form data.</summary>
    public async Task<(UploadPhotoResponse? Response, string? Error)> UploadPhotoAsync(Guid advertId,
        Stream content, string fileName, string contentType, UploadPhotoRequest metadata,
        CancellationToken cancellationToken = default)
    {
        using var form = new MultipartFormDataContent();

        var file = new StreamContent(content);
        file.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        form.Add(file, "file", fileName);

        // form binding on the server matches property names, so the metadata fields travel under those
        AddField(form, nameof(UploadPhotoRequest.PhotoRkid), metadata.PhotoRkid);
        AddField(form, nameof(UploadPhotoRequest.RoomType), metadata.RoomType?.ToString());
        AddField(form, nameof(UploadPhotoRequest.PhotoKind), metadata.PhotoKind?.ToString());
        AddField(form, nameof(UploadPhotoRequest.Order), metadata.Order?.ToString(CultureInfo.InvariantCulture));
        AddField(form, nameof(UploadPhotoRequest.Main), metadata.Main?.ToString(CultureInfo.InvariantCulture));
        AddField(form, nameof(UploadPhotoRequest.Alt), metadata.Alt);

        var response = await httpClient.PostAsync($"{AdvertApiClient.AdvertPath}/{advertId}/photo", form,
            cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<UploadPhotoResponse>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    /// <summary>Removes a photo from an advert; returns only the error message, if any.</summary>
    public async Task<string?> DeletePhotoAsync(Guid advertId, Guid photoId,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.DeleteAsync($"{AdvertApiClient.AdvertPath}/{advertId}/photo/{photoId}",
            cancellationToken);
        return response.IsSuccessStatusCode ? null : await ApiErrorReader.ReadMessageAsync(response);
    }

    private static void AddField(MultipartFormDataContent form, string name, string? value)
    {
        if (value is not null)
        {
            form.Add(new StringContent(value), name);
        }
    }
}
