using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Server.Infrastructure.Http;
using Shared.User;
using Shared.User.UpdateCurrentUser;

namespace Server.Features.User.UpdateCurrentUser;

/// <summary>Lets the signed-in account change its own details.</summary>
public static class UpdateCurrentUser
{
    /// <summary>Registers the PUT /me route.</summary>
    public static void MapUpdateCurrentUser(this IEndpointRouteBuilder group)
    {
        group.MapPut("/me", UpdateCurrentUserAsync)
            .WithName(nameof(UpdateCurrentUserAsync))
            .RequireAuthorization();
    }

    /// <summary>Changes the caller's user name and email; a value held by another account is refused.</summary>
    internal static async Task<IResult> UpdateCurrentUserAsync(UpdateCurrentUserRequest request,
        ClaimsPrincipal principal, UserService userService, UserManager<ApplicationUser> userManager,
        CancellationToken cancellationToken)
    {
        var user = await userService.GetCurrentUserAsync(principal, cancellationToken);
        if (user is null)
        {
            return TypedResults.Unauthorized();
        }

        var nameHolder = await userService.FindUserByNameAsync(request.UserName, cancellationToken);
        if (nameHolder is not null && nameHolder.Id != user.Id)
        {
            return UserErrors.UserNameTaken.ToResult();
        }

        var emailHolder = await userService.FindUserByEmailAsync(request.Email, cancellationToken);
        if (emailHolder is not null && emailHolder.Id != user.Id)
        {
            return UserErrors.EmailTaken.ToResult();
        }

        var nameResult = await userManager.SetUserNameAsync(user, request.UserName);
        if (!nameResult.Succeeded)
        {
            return (UserErrors.ProfileUpdateFailed with
                { Detail = string.Join(" ", nameResult.Errors.Select(e => e.Description)) }).ToResult();
        }

        var emailResult = await userManager.SetEmailAsync(user, request.Email);
        if (!emailResult.Succeeded)
        {
            return (UserErrors.ProfileUpdateFailed with
                { Detail = string.Join(" ", emailResult.Errors.Select(e => e.Description)) }).ToResult();
        }

        return TypedResults.Ok(new UpdateCurrentUserResponse
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty
        });
    }
}
