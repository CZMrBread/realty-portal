using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Server.Features.RealtyAgent;
using Shared.User.GetCurrentUser;

namespace Server.Features.User.GetCurrentUser;

/// <summary>Returns the account behind the caller's token.</summary>
public static class GetCurrentUser
{
    /// <summary>Registers the /me route.</summary>
    public static void MapGetCurrentUser(this IEndpointRouteBuilder group)
    {
        group.MapGet("/me", GetCurrentUserAsync).WithName(nameof(GetCurrentUserAsync));
    }

    /// <summary>The caller's account with its roles, or 401 when the claims name no account.</summary>
    internal static async Task<IResult> GetCurrentUserAsync(ClaimsPrincipal principal,
        UserManager<ApplicationUser> userManager, UserService userService, CancellationToken cancellationToken)
    {
        var user = await userService.GetCurrentUserAsync(principal, cancellationToken);
        if (user == null)
        {
            return Results.Unauthorized();
        }
        var roles = await userService.GetUserRolesAsync(user, cancellationToken);

        var userInfo = new GetCurrentUserResponse
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Roles = roles
        };
        return TypedResults.Ok(userInfo);
    }
}
