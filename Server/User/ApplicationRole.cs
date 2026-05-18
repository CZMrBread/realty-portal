using Microsoft.AspNetCore.Identity;
using Server.Shared;

namespace Server.User;

public class ApplicationRole : IdentityRole<Guid>, ITimeStampedEntity
{
    public const string SuperAdmin = nameof(SuperAdmin);
    public const string Admin = nameof(Admin);
    public const string RealtyAgencyAdmin = nameof(RealtyAgencyAdmin);
    public const string RealtyAgent = nameof(RealtyAgent);
    public const string User = nameof(User);

    public static readonly string[] AllRoles =
    [
        SuperAdmin,
        Admin,
        RealtyAgencyAdmin,
        RealtyAgent,
        User
    ];

    public ApplicationRole()
    {
        Id = Guid.CreateVersion7();
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public override Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}