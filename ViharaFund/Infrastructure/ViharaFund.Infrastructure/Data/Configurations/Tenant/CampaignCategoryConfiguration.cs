using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class CampaignCategoryConfiguration : IEntityTypeConfiguration<CampaignCategory>
    {
        public void Configure(EntityTypeBuilder<CampaignCategory> builder)
        {
            builder.ToTable("CampaignCategory");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();
        }
    }
}
