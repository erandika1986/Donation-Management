using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class JobCardTaskCommentConfiguration : IEntityTypeConfiguration<JobCardTaskComment>
    {
        public void Configure(EntityTypeBuilder<JobCardTaskComment> builder)
        {
            builder.ToTable("JobCardTaskComment");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.IsEdited).HasDefaultValue(false);

            builder.HasOne<JobCardTask>(a => a.JobCardTask)
                   .WithMany(u => u.JobCardTaskComments)
                   .HasForeignKey(a => a.JobCardTaskId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.CreatedByUser)
                   .WithMany(u => u.CreatedJobCardTaskComments)
                   .HasForeignKey(a => a.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.UpdatedByUser)
                   .WithMany(u => u.UpdatedJobCardTaskComments)
                   .HasForeignKey(a => a.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
