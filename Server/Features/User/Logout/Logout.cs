using Server.Infrastructure.Authentication;
using Shared.User;

namespace Server.Features.User.Logout;

/// <summary>Ends the session the caller presents a refresh token for.</summary>
public static class Logout
{
    /// <summary>Registers the /logout route.</summary>
    public static void MapLogout(this IEndpointRouteBuilder group)
    {
        group.MapPost("/logout", LogoutUserAsync)
            .WithName(nameof(LogoutUserAsync))
            .RequireAuthorization();
    }

    /// <summary>
    /// Withdraws the presented refresh token, so that signing out really ends the session instead of only
    /// dropping the tokens from the browser. Only the token that was handed in is withdrawn, which leaves the
    /// user signed in wherever else they are. An unknown token is answered the same as a known one, since there
    /// is nothing for the caller to do about it either way.
    /// </summary>
    private static async Task<IResult> LogoutUserAsync(RefreshTokenRequest refreshTokenRequest,
        AccessTokenService tokenService)
    {
        await tokenService.RevokeAsync(refreshTokenRequest.RefreshToken);
        return TypedResults.NoContent();
    }
}
