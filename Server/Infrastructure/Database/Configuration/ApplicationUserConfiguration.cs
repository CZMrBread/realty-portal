using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.User;

namespace Server.Infrastructure.Database.Configuration;

/// <summary>Maps the user table. The identifier is assigned in code, so the database must not generate one.</summary>
public sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.Id).ValueGeneratedNever();
    }
}
