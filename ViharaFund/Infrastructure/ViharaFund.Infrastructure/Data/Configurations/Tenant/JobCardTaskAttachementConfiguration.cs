using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class JobCardTaskAttachmentConfiguration : IEntityTypeConfiguration<JobCardTaskAttachment>
    {
        public void Configure(EntityTypeBuilder<JobCardTaskAttachment> builder)
        {
            builder.ToTable("JobCardTaskAttachment");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Description)
                .IsRequired(false);

            builder
               .HasOne<JobCardTask>(c => c.JobTask)
               .WithMany(c => c.JobCardTaskAttachments)
               .HasForeignKey(c => c.JobCardTaskId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(true);

            builder.HasOne<User>(a => a.CreatedByUser)
                   .WithMany(u => u.CreatedJobCardTaskAttachments)
                   .HasForeignKey(a => a.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.UpdatedByUser)
                   .WithMany(u => u.UpdatedJobCardTaskAttachments)
                   .HasForeignKey(a => a.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
