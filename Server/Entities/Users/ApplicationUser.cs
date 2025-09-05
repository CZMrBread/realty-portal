using Microsoft.AspNetCore.Identity;

namespace Server.Entities.Users;

public class ApplicationUser : IdentityUser<Guid>, ITimeStampedEntity
{
    public ApplicationUser()
    {
        Id = Guid.CreateVersion7();
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public ApplicationUser(string userName) : this()
    {
        UserName = userName;
    }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}