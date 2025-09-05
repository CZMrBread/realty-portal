using Microsoft.AspNetCore.Identity;

namespace Server.Entities.Users;

public class ApplicationRole : IdentityRole<Guid>, ITimeStampedEntity
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string RealtyAgencyAdmin = "RealtyAgencyAdmin";
    public const string RealtyAgent = "RealtyAgent";
    public const string User = "User";

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