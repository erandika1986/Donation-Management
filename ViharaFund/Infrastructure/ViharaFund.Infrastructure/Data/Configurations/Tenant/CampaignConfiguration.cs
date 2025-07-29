using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
    {
        public void Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.ToTable("Campaign");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.TargetAmount);

            builder.Property(p => p.EndDate).IsRequired(false);

            builder.Property(p => p.CompaignImageUrl).IsRequired(false);

            builder.HasOne<CurrencyType>(a => a.CurrencyType)
                   .WithMany(u => u.Campaigns)
                   .HasForeignKey(a => a.CurrencyTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<CampaignCategory>(a => a.CampaignCategory)
                   .WithMany(u => u.Campaigns)
                   .HasForeignKey(a => a.CampaignCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.CreatedByUser)
                   .WithMany(u => u.CreatedCampaigns)
                   .HasForeignKey(a => a.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            builder.HasOne<User>(a => a.UpdatedByUser)
                   .WithMany(u => u.UpdatedCampaigns)
                   .HasForeignKey(a => a.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);
        }
    }
}
