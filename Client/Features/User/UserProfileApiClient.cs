using System.Net.Http.Json;
using Client.Infrastructure;
using Shared.User.ChangePassword;
using Shared.User.GetCurrentUser;
using Shared.User.GetUserByEmail;
using Shared.User.UpdateCurrentUser;

namespace Client.Features.User;

/// <summary>Calls the /user/me endpoints through the authenticated client.</summary>
public sealed class UserProfileApiClient(HttpClient httpClient)
{
    /// <summary>Reads the account the token belongs to.</summary>
    public async Task<(GetCurrentUserResponse? Response, string? Error)> GetCurrentUserAsync(
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync("user/me", cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<GetCurrentUserResponse>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    /// <summary>Changes the name and email of the calling account.</summary>
    public async Task<(UpdateCurrentUserResponse? Response, string? Error)> UpdateCurrentUserAsync(
        UpdateCurrentUserRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PutAsJsonAsync("user/me", request, cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<UpdateCurrentUserResponse>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    /// <summary>Finds an account by the email it registered with. Agents only, e.g. when taking a colleague on.</summary>
    public async Task<(GetUserByEmailResponse? Response, string? Error)> GetUserByEmailAsync(string email,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync($"user/email/{Uri.EscapeDataString(email)}", cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<GetUserByEmailResponse>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    /// <summary>Replaces the password of the calling account; returns only the error message, if any.</summary>
    public async Task<string?> ChangePasswordAsync(ChangePasswordRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PutAsJsonAsync("user/me/password", request, cancellationToken);
        return response.IsSuccessStatusCode ? null : await ApiErrorReader.ReadMessageAsync(response);
    }
}
