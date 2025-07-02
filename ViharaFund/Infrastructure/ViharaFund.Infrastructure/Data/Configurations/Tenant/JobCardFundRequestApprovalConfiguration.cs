using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class JobCardFundRequestApprovalConfiguration : IEntityTypeConfiguration<JobCardFundRequestApproval>
    {
        public void Configure(EntityTypeBuilder<JobCardFundRequestApproval> builder)
        {
            builder.ToTable("JobCardFundRequestApproval");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Comment)
                .IsRequired(false);

            builder.Property(p => p.ApprovalDate)
                .IsRequired(false);

            builder
               .HasOne<JobCardFundRequest>(c => c.JobCardFundRequest)
               .WithMany(c => c.JobCardFundRequestApprovals)
               .HasForeignKey(c => c.FundRequestId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(true);

            builder
               .HasOne<User>(c => c.Approver)
               .WithMany(c => c.JobCardFundRequestApprovals)
               .HasForeignKey(c => c.ApproverId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(true);

            builder.HasOne<User>(a => a.CreatedByUser)
                   .WithMany(u => u.CreatedJobCardFundRequestApprovals)
                   .HasForeignKey(a => a.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.UpdatedByUser)
                   .WithMany(u => u.UpdatedJobCardFundRequestApprovals)
                   .HasForeignKey(a => a.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
