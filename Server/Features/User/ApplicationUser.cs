using Microsoft.AspNetCore.Identity;
using Server.Infrastructure.Database;

namespace Server.Features.User;

public class ApplicationUser : IdentityUser<Guid>, ITimeStampedEntity
{
    public ApplicationUser(string userName)
    {
        UserName = userName;
    }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = new List<RefreshTokenEntity>();
}