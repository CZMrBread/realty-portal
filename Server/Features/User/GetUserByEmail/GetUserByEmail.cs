using Server.Features.RealtyAgent;
using Server.Infrastructure.Http;
using Shared.User;
using Shared.User.GetUserByEmail;

namespace Server.Features.User.GetUserByEmail;

/// <summary>Finds an account by the email address it registered with.</summary>
public static class GetUserByEmail
{
    /// <summary>
    /// Registers the email lookup route. Only agents may ask, so the portal cannot be used to probe which
    /// email addresses have an account.
    /// </summary>
    public static void MapGetUserByEmail(this IEndpointRouteBuilder group)
    {
        group.MapGet("/email/{email}", GetUserByEmailAsync)
            .WithName(nameof(GetUserByEmailAsync))
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>Returns the account registered under <paramref name="email"/>, or 404 when there is none.</summary>
    internal static async Task<IResult> GetUserByEmailAsync(string email, UserService userService,
        CancellationToken cancellationToken)
    {
        var user = await userService.FindUserByEmailAsync(email, cancellationToken);
        if (user is null)
        {
            return UserErrors.NotFound.ToResult();
        }

        return TypedResults.Ok(new GetUserByEmailResponse
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty
        });
    }
}
