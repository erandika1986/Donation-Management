using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCardApprovalLevel : BaseEntity
    {
        public string LevelName { get; set; }
        public int AssignRoleGroupId { get; set; }

        public virtual Role AssignRoleGroup { get; set; }

        public virtual ICollection<JobCardApproval> JobCardApprovals { get; set; } = new HashSet<JobCardApproval>();
    }
}
