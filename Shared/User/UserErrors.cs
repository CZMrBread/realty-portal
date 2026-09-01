using System.Net;
using Shared.Shared;

namespace Shared.User;

/// <summary>Every way a user request can be refused.</summary>
public static class UserErrors
{
    /// <summary>The email address or password did not match an account.</summary>
    public static readonly ApiError InvalidCredentials =
        new("user.invalid_credentials", HttpStatusCode.Unauthorized, "Wrong email or password.");

    /// <summary>An account already holds the submitted email address.</summary>
    public static readonly ApiError EmailTaken =
        new("user.email_taken", HttpStatusCode.BadRequest, "User with this email already exists.");

    /// <summary>An account already holds the submitted user name.</summary>
    public static readonly ApiError UserNameTaken =
        new("user.username_taken", HttpStatusCode.BadRequest, "Username is already taken.");

    /// <summary>Identity refused to create the account, most often over the password rules.</summary>
    public static readonly ApiError RegistrationFailed =
        new("user.registration_failed", HttpStatusCode.BadRequest, "The account could not be created.");

    /// <summary>The refresh token is unknown, already spent, or expired.</summary>
    public static readonly ApiError InvalidRefreshToken =
        new("user.invalid_refresh_token", HttpStatusCode.BadRequest, "The refresh token is unknown, spent or expired.");

    /// <summary>No account exists with the given identifier.</summary>
    public static readonly ApiError NotFound =
        new("user.not_found", HttpStatusCode.NotFound, "No account exists with that identifier.");

    /// <summary>Identity refused to change the account details.</summary>
    public static readonly ApiError ProfileUpdateFailed =
        new("user.profile_update_failed", HttpStatusCode.BadRequest, "The account details could not be changed.");

    /// <summary>Identity refused the password change, over the current password or the password rules.</summary>
    public static readonly ApiError PasswordChangeFailed =
        new("user.password_change_failed", HttpStatusCode.BadRequest, "The password could not be changed.");
}
