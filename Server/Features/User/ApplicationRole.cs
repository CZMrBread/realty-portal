using Microsoft.AspNetCore.Identity;
using Server.Infrastructure.Database;
using Shared.User;

namespace Server.Features.User;

/// <summary>A user role; the portal-wide roles are seeded at startup from <see cref="UserRoles.All"/>.</summary>
public class ApplicationRole : IdentityRole<Guid>, ITimeStampedEntity
{
    public override Guid Id { get; set; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
