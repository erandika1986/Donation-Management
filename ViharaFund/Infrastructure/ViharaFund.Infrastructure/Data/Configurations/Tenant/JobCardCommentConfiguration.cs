using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class JobCardCommentConfiguration : IEntityTypeConfiguration<JobCardComment>
    {
        public void Configure(EntityTypeBuilder<JobCardComment> builder)
        {
            builder.ToTable("JobCardComment");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.HasOne<JobCard>(a => a.JobCard)
                   .WithMany(u => u.JobCardComments)
                   .HasForeignKey(a => a.JobCardId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.CreatedByUser)
                   .WithMany(u => u.CreatedJobCardComments)
                   .HasForeignKey(a => a.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.UpdatedByUser)
                   .WithMany(u => u.UpdatedJobCardComments)
                   .HasForeignKey(a => a.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
