using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class JobCardFundRequestReleaseConfiguration : IEntityTypeConfiguration<JobCardFundRequestRelease>
    {
        public void Configure(EntityTypeBuilder<JobCardFundRequestRelease> builder)
        {
            builder.ToTable("JobCardFundRequestRelease");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();


            builder.Property(p => p.TransactionNumber)
                .IsRequired(false);

            builder.Property(p => p.PaymentProofUrl)
                .IsRequired(false);


            builder.HasOne<JobCard>(a => a.JobCard)
                   .WithMany(u => u.JobCardFundRequestReleases)
                   .HasForeignKey(a => a.JobCardId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.CreatedByUser)
                   .WithMany(u => u.CreatedJobCardFundRequestReleases)
                   .HasForeignKey(a => a.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.UpdatedByUser)
                   .WithMany(u => u.UpdatedJobCardFundRequestReleases)
                   .HasForeignKey(a => a.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
