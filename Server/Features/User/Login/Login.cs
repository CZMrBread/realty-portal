using Microsoft.AspNetCore.Identity;
using Server.Infrastructure.Authentication;
using Shared.User.Login;

namespace Server.Features.User.Login;

public static class Login
{
    public static void MapLogin(this IEndpointRouteBuilder group)
    {
        group.MapPost("/login", LoginUserAsync).WithName(nameof(LoginUserAsync));
    }

    private static async Task<IResult> LoginUserAsync(LoginUserRequest loginUserRequest,
        UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        JwtTokenGenerator tokenGenerator)
    {            
        var user = await userManager.FindByEmailAsync(loginUserRequest.Email)
                            ?? await userManager.FindByNameAsync(loginUserRequest.Email);
        if (user == null)
        {
            return TypedResults.Unauthorized();
        }
        var result = await signInManager.CheckPasswordSignInAsync(user, loginUserRequest.Password, false);
        if (!result.Succeeded)
        {
            return TypedResults.Unauthorized();
        }
        
        var accessToken = await tokenGenerator.GenerateAccessTokenAsync(user);
        var refreshToken = await tokenGenerator.GenerateRefreshTokenAsync(user);
        var roles = await userManager.GetRolesAsync(user);
        
        var response = new LoginUserResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Roles = roles,
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            AccessTokenExpiration = JwtTokenGenerator.AccessTokenExpiry,
            RefreshTokenExpiration = refreshToken.ExpiresAt
        };
        
        return TypedResults.Ok(response);
    }
}