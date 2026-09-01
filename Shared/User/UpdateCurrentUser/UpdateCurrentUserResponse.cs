namespace Shared.User.UpdateCurrentUser;

/// <summary>The account as it stands after the update.</summary>
public sealed record UpdateCurrentUserResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
