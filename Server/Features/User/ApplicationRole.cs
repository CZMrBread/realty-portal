using Microsoft.AspNetCore.Identity;
using Server.Infrastructure.Database;
using Shared.User;

namespace Server.Features.User;

/// <summary>A role a user account can hold. The portal-wide roles are created at startup from <see cref="UserRoles.All"/>.</summary>
public class ApplicationRole : IdentityRole<Guid>, ITimeStampedEntity
{
    public override Guid Id { get; set; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
