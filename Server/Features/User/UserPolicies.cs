namespace Server.Features.User;

/// <summary>
/// Names of the authorization policies built on the portal-wide roles, so that an endpoint and the registration
/// in the composition root refer to the same string. The policies themselves are defined in Program.cs.
/// </summary>
public static class UserPolicies
{
    /// <summary>Only a caller who administers the whole portal.</summary>
    public const string SuperAdminOnly = nameof(SuperAdminOnly);
}
