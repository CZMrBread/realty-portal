using Microsoft.AspNetCore.Identity;
using Server.Infrastructure.Authentication;
using Shared.User;
using Server.Infrastructure.Http;
using Shared.User.Register;

namespace Server.Features.User.Register;

/// <summary>Creates a new account and signs it in straight away.</summary>
public static class Register
{
    /// <summary>Registers the /register route.</summary>
    public static void MapRegister(this IEndpointRouteBuilder group)
    {
        group.MapPost("/register", RegisterUserAsync).WithName(nameof(RegisterUserAsync));
    }

    /// <summary>Rejects an email address or username that is already taken, then creates the account and issues its first pair of tokens.</summary>
    private static async Task<IResult> RegisterUserAsync(RegisterUserRequest registerUserRequest,
        UserService userService, UserManager<ApplicationUser> userManager,
        AccessTokenService tokenService, CancellationToken cancellationToken)
    {
        var existingUser = await userService.FindUserByEmailAsync(registerUserRequest.Email, cancellationToken);
        if (existingUser != null)
        {
            return UserErrors.EmailTaken.ToResult();
        }

        existingUser = await userService.FindUserByNameAsync(registerUserRequest.UserName, cancellationToken);
        if (existingUser != null)
        {
            return UserErrors.UserNameTaken.ToResult();
        }

        var user = new ApplicationUser(registerUserRequest.UserName)
        {
            Email = registerUserRequest.Email
        };

        var result = await userService.CreateUserAsync(user, registerUserRequest.Password, cancellationToken);
        if (!result.Succeeded)
        {
            return (UserErrors.RegistrationFailed with
                { Detail = string.Join(" ", result.Errors.Select(e => e.Description)) }).ToResult();
        }

        var token = await tokenService.CreateAuthenticationAsync(user);
        if (token == null)
        {
            return UserErrors.InvalidCredentials.ToResult();
        }
        var response = new RegisterUserResponse
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