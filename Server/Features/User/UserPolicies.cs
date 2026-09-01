namespace Server.Features.User;

/// <summary>Names of the role-based authorization policies; the policies are defined in Program.cs.</summary>
public static class UserPolicies
{
    /// <summary>Only a portal-wide administrator.</summary>
    public const string SuperAdminOnly = nameof(SuperAdminOnly);
}
