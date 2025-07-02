using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class JobCardFundRequestConfiguration : IEntityTypeConfiguration<JobCardFundRequest>
    {
        public void Configure(EntityTypeBuilder<JobCardFundRequest> builder)
        {
            builder.ToTable("JobCardFundRequest");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.ReleaseAmount)
                .IsRequired(false);

            builder.Property(p => p.ReleasedOn)
                .IsRequired(false);

            builder.Property(p => p.Note)
                .IsRequired(false);

            builder
               .HasOne<JobCard>(c => c.JobCard)
               .WithMany(c => c.JobCardFundRequests)
               .HasForeignKey(c => c.JobCardId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(true);

            builder
               .HasOne<User>(c => c.RequestedBy)
               .WithMany(c => c.JobCardFundRequests)
               .HasForeignKey(c => c.RequestedById)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(true);

            builder.HasOne<User>(a => a.CreatedByUser)
                   .WithMany(u => u.CreatedJobCardFundRequests)
                   .HasForeignKey(a => a.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.UpdatedByUser)
                   .WithMany(u => u.UpdatedJobCardFundRequests)
                   .HasForeignKey(a => a.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
