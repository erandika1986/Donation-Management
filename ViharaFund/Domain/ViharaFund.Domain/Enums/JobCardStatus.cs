using System.ComponentModel;

namespace ViharaFund.Domain.Enums
{
    public enum JobCardStatus
    {
        [Description("Draft")]
        Draft = 1,

        [Description("Pending Approval")]
        PendingApproval = 2,

        [Description("Partially Approved")]
        PartiallyApproved = 3,

        [Description("Approved")]
        Approved = 4,

        [Description("On Going")]
        OnGoing = 5,

        [Description("Pending On Hold")]
        PendingOnHold = 6,

        [Description("On Hold")]
        OnHold = 7,

        [Description("Rejected")]
        Rejected = 8,

        [Description("Pending Cancellation")]
        PendingCancellation = 9,

        [Description("Cancelled")]
        Cancelled = 10,

        [Description("Pending Completion")]
        PendingCompletion = 11,

        [Description("Completed")]
        Completed = 12
    }
}
