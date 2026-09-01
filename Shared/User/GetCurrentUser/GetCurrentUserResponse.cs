namespace Shared.User.GetCurrentUser;

/// <summary>Identity of the signed-in user, read from the presented token.</summary>
public sealed record GetCurrentUserResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    /// <summary>Roles the user holds.</summary>
    public IList<string> Roles { get; set; } = new List<string>();
    
}