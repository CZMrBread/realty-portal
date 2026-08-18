using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Features.User.GetCurrentUser;
using Server.Features.User.GetUserProfile;
using Server.Features.User.Login;
using Server.Features.User.Register;
using Server.Infrastructure.Authentication;
using Shared.User;

namespace Server.Features.User;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup("/user");
        userGroup.MapLogin();
        userGroup.MapRegister();
        userGroup.MapGetUserProfile();
        userGroup.MapGetCurrentUser();
    }
    
    public static async Task<IResult> RefreshTokenAsync(
        RefreshTokenDto refreshTokenDto,
        JwtTokenGenerator tokenGenerator,
        UserManager<ApplicationUser> userManager)
    {
        try
        {
            var (accessToken, refreshToken) = await tokenGenerator.RefreshTokenAsync(refreshTokenDto.RefreshToken);
            await tokenGenerator.RevokeTokenAsync(refreshTokenDto.RefreshToken);

            var user = refreshToken.User;
            var roles = await userManager.GetRolesAsync(user);

            var response = new UserAuthenticationDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(15),
                RefreshTokenExpiration = refreshToken.ExpiresAt,
                User = new UserInfoDto
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Roles = roles
                }
            };

            return Results.Ok(response);
        }
        catch (SecurityTokenException)
        {
            return Results.Unauthorized();
        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }
    }

    public static async Task<IResult> RevokeTokenAsync(
        RefreshTokenDto refreshTokenDto,
        JwtTokenGenerator tokenGenerator,
        ClaimsPrincipal user)
    {
        try
        {
            var result = await tokenGenerator.RevokeTokenAsync(refreshTokenDto.RefreshToken);

            if (!result)
            {
                return Results.BadRequest(new { message = "Token not found or already revoked." });
            }

            return Results.Ok(new { message = "Token revoked successfully." });
        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }
    }

    public static async Task<IResult> GetCurrentUserAsync(
        ClaimsPrincipal principal,
        UserManager<ApplicationUser> userManager)
    {
        try
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || !Guid.TryParse(userId, out var userGuid))
            {
                return Results.Unauthorized();
            }

            var user = await userManager.FindByIdAsync(userGuid.ToString());
            if (user == null)
            {
                return Results.NotFound(new { message = "User not found." });
            }

            var roles = await userManager.GetRolesAsync(user);

            var userInfo = new UserInfoDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Roles = roles
            };

            return Results.Ok(userInfo);
        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }
    }

    public static async Task<IResult> LogoutAsync(
        ClaimsPrincipal principal,
        JwtTokenGenerator tokenGenerator)
    {
        try
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || !Guid.TryParse(userId, out var userGuid))
            {
                return Results.Unauthorized();
            }

            await tokenGenerator.RevokeAllUserTokensAsync(userGuid);

            return Results.Ok(new { message = "Logged out successfully." });
        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }
    }

    public static async Task<IResult> AssignRoleAsync(
        [FromBody] AssignRoleRequest request,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        try
        {
            var user = await userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return Results.NotFound(new { message = "User not found." });
            }

            if (!await roleManager.RoleExistsAsync(request.RoleName))
            {
                return Results.BadRequest(new { message = "Role does not exist." });
            }

            var result = await userManager.AddToRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
            {
                return Results.BadRequest(new { message = "Failed to assign role.", errors = result.Errors });
            }

            return Results.Ok(new { message = $"Role '{request.RoleName}' assigned to user successfully." });
        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }
    }

    public record AssignRoleRequest(Guid UserId, string RoleName);
}