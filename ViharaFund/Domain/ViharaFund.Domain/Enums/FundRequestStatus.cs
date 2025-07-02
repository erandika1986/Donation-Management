using System.ComponentModel;

namespace ViharaFund.Domain.Enums
{
    public enum FundRequestStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Approved")]
        Approved = 2,
        [Description("Rejected")]
        Rejected = 3,
        [Description("Partially Paid")]
        PartiallyPaid = 4,
        [Description("Fully Paid")]
        FullyPaid = 5,
    }
}
