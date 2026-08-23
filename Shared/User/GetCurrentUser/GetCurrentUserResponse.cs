namespace Shared.User.GetCurrentUser;

/// <summary>Identity of the signed-in user, as told by the token they presented.</summary>
public sealed record GetCurrentUserResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    /// <summary>Roles the user holds, which decide what they are allowed to do.</summary>
    public IList<string> Roles { get; set; } = new List<string>();
    
}