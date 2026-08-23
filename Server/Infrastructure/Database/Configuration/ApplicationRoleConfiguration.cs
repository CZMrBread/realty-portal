using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.User;

namespace Server.Infrastructure.Database.Configuration;

/// <summary>Maps the role table. The identifier is assigned in code, so the database must not generate one.</summary>
public sealed class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.Property(r => r.Id).ValueGeneratedNever();
    }
}