using System.ComponentModel;

namespace ViharaFund.Domain.Enums
{
    public enum JobCardStatus
    {
        [Description("Draft")]
        Draft = 1,
        [Description("Submitted")]
        Submitted = 2,
        [Description("Approved")]
        Approved = 3,
        [Description("On Going")]
        OnGoing = 4,
        [Description("On Hold")]
        OnHold = 5,
        [Description("Rejected")]
        Rejected = 6,
        [Description("Cancelled")]
        Cancelled = 7,
        [Description("Completed")]
        Completed = 8
    }


}
