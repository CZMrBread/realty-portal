using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Server.Infrastructure.Database;
using Server.Infrastructure.Http;
using Shared.User;
using Shared.User.GetUserProfile;

namespace Server.Features.User.GetUserProfile;

/// <summary>Returns the public profile of a user.</summary>
public static class GetUserProfile
{
    /// <summary>Registers the route that reads a user by identifier.</summary>
    public static void MapGetUserProfile(this IEndpointRouteBuilder group)
    {
        group.MapGet("/{id:guid}", GetUserProfileAsync).WithName(nameof(GetUserProfileAsync));
    }

    /// <summary>Returns the profile of the named user, or 404 when no such account exists.</summary>
    private static async Task<IResult> GetUserProfileAsync(Guid id, UserManager<ApplicationUser> userManager)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return UserErrors.NotFound.ToResult();
        }

        var userProfile = new GetUserProfileResponse
        {
        };

        return TypedResults.Ok(userProfile);
    }
}