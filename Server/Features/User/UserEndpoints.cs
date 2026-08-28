using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Features.User.GetCurrentUser;
using Server.Features.User.GetUserProfile;
using Server.Features.User.Login;
using Server.Features.User.Logout;
using Server.Features.User.Refresh;
using Server.Features.User.Register;
using Server.Infrastructure.Authentication;
using Shared.User;

namespace Server.Features.User;

/// <summary>Collects every route of the User feature under one group.</summary>
public static class UserEndpoints
{
    /// <summary>Path every route of the feature hangs under.</summary>
    public const string Prefix = "/user";

    /// <summary>OpenAPI tag the routes are listed under, so that they show up as one category.</summary>
    public const string Tag = "User";

    /// <summary>Registers the sign-in, sign-out, registration, profile and token refresh routes under <see cref="Prefix"/>.</summary>
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup(Prefix).WithTags(Tag);
        userGroup.MapLogin();
        userGroup.MapRegister();
        userGroup.MapGetUserProfile();
        userGroup.MapGetCurrentUser();
        userGroup.MapRefresh();
        userGroup.MapLogout();
    }
}