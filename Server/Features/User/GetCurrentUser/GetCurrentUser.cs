using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Server.Infrastructure.Database;
using Shared.Shared;
using Shared.User.GetCurrentUser;
using Shared.User.GetUserProfile;

namespace Server.Features.User.GetCurrentUser;

public static class GetCurrentUser
{
    public static void MapGetCurrentUser(this IEndpointRouteBuilder group)
    {
        group.MapGet("/me", GetCurrentUserAsync).WithName(nameof(GetCurrentUserAsync));
    }

    private static async Task<IResult> GetCurrentUserAsync(ClaimsPrincipal principal,
        UserManager<ApplicationUser> userManager)
    {
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null || !Guid.TryParse(userId, out var userGuid))
        {
            return Results.Unauthorized();
        }

        var user = await userManager.FindByIdAsync(userGuid.ToString());
        if (user == null)
        {
            return Results.NotFound(new ErrorMessage
            {
                MessageApi = "User not found.",
                MessageCz = "Uživatel nenalezen.",
                MessageEn = "User not found."
            });
        }

        var roles = await userManager.GetRolesAsync(user);

        var userInfo = new GetCurrentUserResponse
        {

        };
        return TypedResults.Ok(userInfo);
    }
}