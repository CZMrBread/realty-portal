using Microsoft.AspNetCore.Identity;
using Server.Entities;
using Server.Entities.Users;
using Server.Shared;

namespace Server.User;

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

    public ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = new List<RefreshTokenEntity>();
}