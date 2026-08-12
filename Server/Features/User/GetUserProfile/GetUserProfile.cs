using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Server.Infrastructure.Database;
using Shared.User.GetUserProfile;

namespace Server.Features.User.GetUserProfile;

public static class GetUserProfile
{
    public static void MapGetUserProfile(this IEndpointRouteBuilder group)
    {
        group.MapGet("/{id:guid}", GetUserProfileAsync).WithName(nameof(GetUserProfileAsync));
    }

    private static async Task<IResult> GetUserProfileAsync(Guid id, UserManager<ApplicationUser> userManager)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return TypedResults.NotFound();
        }

        var userProfile = new GetUserProfileResponse
        {
        };

        return TypedResults.Ok(userProfile);
    }
}