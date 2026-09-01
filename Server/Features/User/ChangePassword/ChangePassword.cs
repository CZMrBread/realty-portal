using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Server.Infrastructure.Http;
using Shared.User;
using Shared.User.ChangePassword;

namespace Server.Features.User.ChangePassword;

/// <summary>Lets the signed-in account replace its own password.</summary>
public static class ChangePassword
{
    /// <summary>Registers the PUT /me/password route.</summary>
    public static void MapChangePassword(this IEndpointRouteBuilder group)
    {
        group.MapPut("/me/password", ChangePasswordAsync)
            .WithName(nameof(ChangePasswordAsync))
            .RequireAuthorization();
    }

    /// <summary>Replaces the caller's password after Identity verifies the current one.</summary>
    internal static async Task<IResult> ChangePasswordAsync(ChangePasswordRequest request,
        ClaimsPrincipal principal, UserService userService, UserManager<ApplicationUser> userManager,
        CancellationToken cancellationToken)
    {
        var user = await userService.GetCurrentUserAsync(principal, cancellationToken);
        if (user is null)
        {
            return TypedResults.Unauthorized();
        }

        var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            return (UserErrors.PasswordChangeFailed with
                { Detail = string.Join(" ", result.Errors.Select(e => e.Description)) }).ToResult();
        }

        return TypedResults.NoContent();
    }
}
