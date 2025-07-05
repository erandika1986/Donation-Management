using System.ComponentModel;

namespace ViharaFund.Domain.Enums
{
    public enum JobCardStatus
    {
        [Description("Draft")]
        Draft = 1,
        [Description("Pending Approval")]
        PendingApproval = 2,
        [Description("Approved")]
        Approved = 3,
        [Description("On Going")]
        OnGoing = 4,
        [Description("On Hold")]
        OnHold = 5,
        [Description("Pending On Hold")]
        PendingOnHold = 6,
        [Description("Rejected")]
        Rejected = 7,
        [Description("Pending Cancellation")]
        PendingCancellation = 8,
        [Description("Cancelled")]
        Cancelled = 9,
        [Description("Pending Completion")]
        PendingCompletion = 10,
        [Description("Completed")]
        Completed = 11
    }


}
