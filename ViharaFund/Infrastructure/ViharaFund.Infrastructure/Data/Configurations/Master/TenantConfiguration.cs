using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ViharaFund.Infrastructure.Data.Configurations.Master
{
    internal class TenantConfiguration : IEntityTypeConfiguration<ViharaFund.Domain.Entities.Master.Tenant>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Master.Tenant> builder)
        {
            builder.ToTable("Tenant");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();
        }
    }
}
