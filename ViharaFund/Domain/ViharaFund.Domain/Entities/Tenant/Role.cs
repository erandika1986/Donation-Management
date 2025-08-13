using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        public virtual ICollection<JobCard> AssignedJobCards { get; set; } = new HashSet<JobCard>();
        public virtual ICollection<JobCardApprovalLevel> JobCardApprovalLevels { get; set; } = new HashSet<JobCardApprovalLevel>();
        public virtual ICollection<Group> Groups { get; set; } = new HashSet<Group>();
    }
}
