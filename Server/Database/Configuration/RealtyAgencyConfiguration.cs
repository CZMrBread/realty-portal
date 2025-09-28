using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Entities.RealtyAgency;

namespace Server.Database.Configuration;

public sealed class RealtyAgencyConfiguration: IEntityTypeConfiguration<RealtyAgencyEntity>
{
    public void Configure(EntityTypeBuilder<RealtyAgencyEntity> builder)
    {
    }
}