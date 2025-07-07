using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class JobCardFundRequestCommentConfiguration : IEntityTypeConfiguration<JobCardFundRequestComment>
    {
        public void Configure(EntityTypeBuilder<JobCardFundRequestComment> builder)
        {
            builder.ToTable("JobCardFundRequestComment");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.HasOne<JobCardFundRequest>(a => a.JobCardFundRequest)
                   .WithMany(u => u.JobCardFundRequestComments)
                   .HasForeignKey(a => a.JobCardFundRequestId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.CreatedByUser)
                   .WithMany(u => u.CreatedJobCardFundRequestComments)
                   .HasForeignKey(a => a.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.UpdatedByUser)
                   .WithMany(u => u.UpdatedJobCardFundRequestComments)
                   .HasForeignKey(a => a.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
