using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.RealtyAgency;

namespace Server.Shared.Database.Configuration;

public sealed class RealtyAgencyConfiguration : IEntityTypeConfiguration<RealtyAgencyEntity>
{
    public void Configure(EntityTypeBuilder<RealtyAgencyEntity> builder)
    {
    }
}