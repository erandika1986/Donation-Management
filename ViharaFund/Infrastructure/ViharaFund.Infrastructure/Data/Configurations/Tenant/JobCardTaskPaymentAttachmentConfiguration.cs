using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class JobCardTaskPaymentAttachmentConfiguration : IEntityTypeConfiguration<JobCardTaskPaymentAttachment>
    {
        public void Configure(EntityTypeBuilder<JobCardTaskPaymentAttachment> builder)
        {
            builder.ToTable("JobCardTaskPaymentAttachment");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder
                .HasOne<JobCardTaskPayment>(c => c.JobCardTaskPayment)
                .WithMany(c => c.JobCardTaskPaymentAttachments)
                .HasForeignKey(c => c.JobCardTaskPaymentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);
        }
    }
}
