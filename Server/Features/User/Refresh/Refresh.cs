using Microsoft.AspNetCore.Identity;
using Server.Infrastructure.Authentication;
using Server.Infrastructure.Http;
using Shared.User;
using Shared.User.Register;

namespace Server.Features.User.Refresh;

/// <summary>Exchanges a refresh token for a fresh pair of tokens.</summary>
public static class Refresh
{
    /// <summary>Registers the /refresh route.</summary>
    public static void MapRefresh(this IEndpointRouteBuilder group)
    {
        group.MapPost("/refresh", RefreshTokenAsync).WithName(nameof(RefreshTokenAsync));
    }

    /// <summary>Issues a new pair of tokens, or answers 400 when the presented refresh token is unknown, spent or expired.</summary>
    private static async Task<IResult> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest,
        UserService userService, UserManager<ApplicationUser> userManager,
        AccessTokenService tokenService, CancellationToken cancellationToken)
    {
        var result = await tokenService.RefreshAsync(refreshTokenRequest.RefreshToken);
        if (result == null)
        {
            return UserErrors.InvalidRefreshToken.ToResult();
        }

        return Results.Ok(result);
    }
}