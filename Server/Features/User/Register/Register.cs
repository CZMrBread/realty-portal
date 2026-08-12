using Microsoft.AspNetCore.Identity;
using Server.Infrastructure.Authentication;
using Shared.User;
using Shared.User.Register;

namespace Server.Features.User.Register;

public static class Register
{
    public static void MapRegister(this IEndpointRouteBuilder group)
    {
        group.MapPost("/register", RegisterUserAsync).WithName(nameof(RegisterUserAsync));
    }

    private static async Task<IResult> RegisterUserAsync(RegisterUserRequest registerUserRequest,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        JwtTokenGenerator tokenGenerator)
    {
        var existingUser = await userManager.FindByEmailAsync(registerUserRequest.Email);
        if (existingUser != null)
        {
            return Results.BadRequest(new { message = "User with this email already exists." });
        }

        existingUser = await userManager.FindByNameAsync(registerUserRequest.UserName);
        if (existingUser != null)
        {
            return Results.BadRequest(new { message = "Username is already taken." });
        }

        var user = new ApplicationUser(registerUserRequest.UserName)
        {
            Email = registerUserRequest.Email
        };

        var result = await userManager.CreateAsync(user, registerUserRequest.Password);
        if (!result.Succeeded)
        {
            return Results.BadRequest(new { message = "Failed to create user.", errors = result.Errors });
        }

        if (!await roleManager.RoleExistsAsync(ApplicationRole.User))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = ApplicationRole.User });
        }

        await userManager.AddToRoleAsync(user, ApplicationRole.User);

        var accessToken = await tokenGenerator.GenerateAccessTokenAsync(user);
        var refreshToken = await tokenGenerator.GenerateRefreshTokenAsync(user);
        var roles = await userManager.GetRolesAsync(user);

        var response = new UserAuthenticationDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            AccessTokenExpiration = JwtTokenGenerator.AccessTokenExpiry,
            RefreshTokenExpiration = refreshToken.ExpiresAt,
            User = new UserInfoDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Roles = roles
            }
        };
        return TypedResults.Ok(response);
    }
}