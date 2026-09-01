using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Features.User.ChangePassword;
using Server.Features.User.GetCurrentUser;
using Server.Features.User.GetUserByEmail;
using Server.Features.User.GetUserProfile;
using Server.Features.User.Login;
using Server.Features.User.Logout;
using Server.Features.User.Refresh;
using Server.Features.User.Register;
using Server.Features.User.UpdateCurrentUser;
using Server.Infrastructure.Authentication;
using Shared.User;

namespace Server.Features.User;

/// <summary>Routes of the User feature.</summary>
public static class UserEndpoints
{
    /// <summary>Route prefix of the feature.</summary>
    public const string Prefix = "/user";

    /// <summary>OpenAPI tag of the feature's routes.</summary>
    public const string Tag = "User";

    /// <summary>Registers the User routes under <see cref="Prefix"/>.</summary>
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup(Prefix).WithTags(Tag);
        userGroup.MapLogin();
        userGroup.MapRegister();
        userGroup.MapGetUserProfile();
        userGroup.MapGetUserByEmail();
        userGroup.MapGetCurrentUser();
        userGroup.MapUpdateCurrentUser();
        userGroup.MapChangePassword();
        userGroup.MapRefresh();
        userGroup.MapLogout();
    }
}