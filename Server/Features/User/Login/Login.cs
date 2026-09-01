using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Server.Infrastructure.Authentication;
using Server.Infrastructure.Http;
using Shared.User;
using Shared.User.Login;

namespace Server.Features.User.Login;

/// <summary>Signs a user in and issues their first token pair.</summary>
public static class Login
{
    /// <summary>Registers the /login route.</summary>
    public static void MapLogin(this IEndpointRouteBuilder group)
    {
        group.MapPost("/login", LoginUserAsync).WithName(nameof(LoginUserAsync));
    }

    /// <summary>Issues tokens for valid credentials; the email also matches a user name, any failure is 401.</summary>
    internal static async Task<IResult> LoginUserAsync(LoginUserRequest loginUserRequest,
        UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        UserService userService, AccessTokenService tokenService, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(loginUserRequest.Email)
                   ?? await userManager.FindByNameAsync(loginUserRequest.Email);
        if (user == null)
        {
            return UserErrors.InvalidCredentials.ToResult();
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, loginUserRequest.Password, false);
        if (!result.Succeeded)
        {
            return UserErrors.InvalidCredentials.ToResult();
        }

        var token = await tokenService.CreateAuthenticationAsync(user);
        if (token == null)
        {
            return UserErrors.InvalidCredentials.ToResult();
        }

        var response = new LoginUserResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Roles = await userService.GetUserRolesAsync(user, cancellationToken),
            Token = token
        };
        return TypedResults.Ok(response);
    }
}
