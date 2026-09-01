using Server.Infrastructure.Authentication;
using Shared.User;

namespace Server.Features.User.Logout;

/// <summary>Ends the session of the presented refresh token.</summary>
public static class Logout
{
    /// <summary>Registers the /logout route.</summary>
    public static void MapLogout(this IEndpointRouteBuilder group)
    {
        group.MapPost("/logout", LogoutUserAsync)
            .WithName(nameof(LogoutUserAsync))
            .RequireAuthorization();
    }

    /// <summary>Revokes only the presented refresh token; an unknown token is answered like a known one.</summary>
    internal static async Task<IResult> LogoutUserAsync(RefreshTokenRequest refreshTokenRequest,
        AccessTokenService tokenService)
    {
        await tokenService.RevokeAsync(refreshTokenRequest.RefreshToken);
        return TypedResults.NoContent();
    }
}
