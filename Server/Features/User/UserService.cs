using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Server.Features.User;

/// <summary>
/// The one place the portal reads and writes accounts. Nothing here is cached: every lookup goes to Identity,
/// which is already backed by the EF change tracker, and a role list is a single indexed join against endpoints
/// that are already paying for a password hash.
/// </summary>
public sealed class UserService(UserManager<ApplicationUser> userManager)
{
    // --- Get ---

    /// <summary>
    /// Account behind a set of claims, or null when the claims carry no usable identifier or name no existing account.
    /// This is the one place the identifier is read out of the token, so that endpoints do not each parse claims themselves.
    /// </summary>
    public async Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal principal,
        CancellationToken cancellationToken = default)
    {
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return null;
        }

        return await FindUserByIdAsync(userId, cancellationToken);
    }

    /// <summary>Account with the given identifier, or null when there is none.</summary>
    public async Task<ApplicationUser?> FindUserByIdAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await userManager.FindByIdAsync(userId.ToString());
    }

    /// <summary>Account with the given email address, or null when there is none.</summary>
    public async Task<ApplicationUser?> FindUserByEmailAsync(string email,
        CancellationToken cancellationToken = default)
    {
        return await userManager.FindByEmailAsync(email);
    }

    /// <summary>Account with the given user name, or null when there is none.</summary>
    public async Task<ApplicationUser?> FindUserByNameAsync(string name,
        CancellationToken cancellationToken = default)
    {
        return await userManager.FindByNameAsync(name);
    }

    /// <summary>Roles the account currently holds.</summary>
    public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user,
        CancellationToken cancellationToken = default)
    {
        return await userManager.GetRolesAsync(user);
    }

    // --- Create / Update / Delete ---

    /// <summary>Creates an account with the given password. A rejected password or a duplicate account comes back in the result rather than as an exception.</summary>
    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password,
        CancellationToken cancellationToken = default)
    {
        return await userManager.CreateAsync(user, password);
    }

    /// <summary>Saves changes to an account.</summary>
    /// <exception cref="InvalidOperationException">Identity refused the change.</exception>
    public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                $"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return user;
    }

    /// <summary>Adds the account to a role.</summary>
    public async Task<IdentityResult> AssignRoleAsync(ApplicationUser user, string role,
        CancellationToken cancellationToken = default)
    {
        return await userManager.AddToRoleAsync(user, role);
    }

    /// <summary>Takes the account out of a role.</summary>
    public async Task<IdentityResult> RemoveRoleAsync(ApplicationUser user, string role,
        CancellationToken cancellationToken = default)
    {
        return await userManager.RemoveFromRoleAsync(user, role);
    }

    /// <summary>Deletes an account.</summary>
    /// <exception cref="InvalidOperationException">Identity refused the deletion.</exception>
    public async Task DeleteUserAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                $"Failed to delete user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}
