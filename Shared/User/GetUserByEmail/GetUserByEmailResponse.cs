namespace Shared.User.GetUserByEmail;

/// <summary>An account found by its email address, with just enough to identify it to the caller.</summary>
public sealed record GetUserByEmailResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
