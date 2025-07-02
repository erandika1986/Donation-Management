using System.ComponentModel;

namespace ViharaFund.Domain.Enums
{
    public enum TaskStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("On Going")]
        OnGoing = 2,
        [Description("On Hold")]
        OnHold = 3,
        [Description("Cancelled")]
        Cancelled = 4,
        [Description("Completed")]
        Completed = 5
    }
}
