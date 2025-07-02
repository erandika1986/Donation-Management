using System.ComponentModel;

namespace ViharaFund.Domain.Enums
{
    public enum JobCardApprovalStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Approved")]
        Approved = 2,
        [Description("Rejected")]
        Rejected = 3
    }
}
