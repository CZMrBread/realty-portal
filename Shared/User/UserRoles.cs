namespace Shared.User;

/// <summary>
/// Portal-wide roles, as opposed to the agent role which is scoped to one agency. Shared so that the client
/// tests the same strings the server puts into the token.
/// </summary>
public static class UserRoles
{
    /// <summary>Role that may administer the whole portal, across every agency.</summary>
    public const string SuperAdmin = nameof(SuperAdmin);

    /// <summary>Every role the portal makes sure exists when it starts.</summary>
    public static readonly string[] All = [SuperAdmin];
}
