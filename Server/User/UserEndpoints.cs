using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared.User;

namespace Server.User;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var authGroup = app.MapGroup("/auth").WithTags("Authentication");

        authGroup.MapPost("/register", RegisterAsync)
            .WithName(nameof(RegisterAsync))
            .WithOpenApi()
            .WithSummary("Register a new user");

        authGroup.MapPost("/login", LoginAsync)
            .WithName(nameof(LoginAsync))
            .WithOpenApi()
            .WithSummary("Login user");

        authGroup.MapPost("/refresh", RefreshTokenAsync)
            .WithName(nameof(RefreshTokenAsync))
            .WithOpenApi()
            .WithSummary("Refresh access token");

        authGroup.MapPost("/revoke", RevokeTokenAsync)
            .WithName(nameof(RevokeTokenAsync))
            .WithOpenApi()
            .RequireAuthorization()
            .WithSummary("Revoke refresh token");

        authGroup.MapGet("/me", GetCurrentUserAsync)
            .WithName(nameof(GetCurrentUserAsync))
            .WithOpenApi()
            .RequireAuthorization()
            .WithSummary("Get current user information");

        authGroup.MapPost("/logout", LogoutAsync)
            .WithName(nameof(LogoutAsync))
            .WithOpenApi()
            .RequireAuthorization()
            .WithSummary("Logout user");

        authGroup.MapPost("/assign-role", AssignRoleAsync)
            .WithName(nameof(AssignRoleAsync))
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole(ApplicationRole.SuperAdmin, ApplicationRole.Admin))
            .WithSummary("Assign role to user");
    }

    public static async Task<IResult> RegisterAsync(
        UserRegistrationDto registrationDto,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        JwtTokenGenerator tokenGenerator)
    {
        try
        {
            var existingUser = await userManager.FindByEmailAsync(registrationDto.Email);
            if (existingUser != null)
            {
                return Results.BadRequest(new { message = "User with this email already exists." });
            }

            existingUser = await userManager.FindByNameAsync(registrationDto.UserName);
            if (existingUser != null)
            {
                return Results.BadRequest(new { message = "Username is already taken." });
            }

            var user = new ApplicationUser(registrationDto.UserName)
            {
                Email = registrationDto.Email
            };

            var result = await userManager.CreateAsync(user, registrationDto.Password);
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
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }
    }

    public static async Task<IResult> LoginAsync(
        UserLoginDto loginDto,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        JwtTokenGenerator tokenGenerator)
    {
        try
        {
            var user = await userManager.FindByNameAsync(loginDto.Username)
                       ?? await userManager.FindByEmailAsync(loginDto.Username);

            if (user == null)
            {
                return Results.Unauthorized();
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Results.Unauthorized();
            }

            var accessToken = await tokenGenerator.GenerateAccessTokenAsync(user);
            var refreshToken = await tokenGenerator.GenerateRefreshTokenAsync(user);
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
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }
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
        catch (SecurityTokenException ex)
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