using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class AppSettingConfiguration : IEntityTypeConfiguration<AppSetting>
    {
        public void Configure(EntityTypeBuilder<AppSetting> builder)
        {
            builder.ToTable("AppSetting");

            builder.HasKey(p => p.Name);
        }
    }
}
