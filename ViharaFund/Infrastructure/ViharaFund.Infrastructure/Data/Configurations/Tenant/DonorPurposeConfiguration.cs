using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class DonorPurposeConfiguration : IEntityTypeConfiguration<DonorPurpose>
    {
        public void Configure(EntityTypeBuilder<DonorPurpose> builder)
        {
            builder.ToTable("DonorPurpose");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();
        }
    }
}
