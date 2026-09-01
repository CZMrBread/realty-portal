namespace Shared.User;

/// <summary>Portal-wide role names, the same strings the server puts into the token.</summary>
public static class UserRoles
{
    /// <summary>Role that may administer the whole portal.</summary>
    public const string SuperAdmin = nameof(SuperAdmin);

    /// <summary>Roles the portal ensures exist at startup.</summary>
    public static readonly string[] All = [SuperAdmin];
}
