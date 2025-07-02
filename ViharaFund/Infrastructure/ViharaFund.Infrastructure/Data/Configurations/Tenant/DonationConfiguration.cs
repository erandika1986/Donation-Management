using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class DonationConfiguration : IEntityTypeConfiguration<Donation>
    {
        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            builder.ToTable("Donation");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder
               .HasOne<Donor>(c => c.Donor)
               .WithMany(c => c.Donations)
               .HasForeignKey(c => c.DonorId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(true);

            builder
               .HasOne<DonorPurpose>(c => c.DonorPurpose)
               .WithMany(c => c.Donations)
               .HasForeignKey(c => c.PurposeId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(true);

            builder.HasOne<User>(a => a.CreatedByUser)
                   .WithMany(u => u.CreatedDonations)
                   .HasForeignKey(a => a.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false); ;

            builder.HasOne<User>(a => a.UpdatedByUser)
                   .WithMany(u => u.UpdatedDonations)
                   .HasForeignKey(a => a.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false); ;
        }
    }
}
