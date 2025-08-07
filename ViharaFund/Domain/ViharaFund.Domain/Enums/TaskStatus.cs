using System.ComponentModel;

namespace ViharaFund.Domain.Enums
{
    public enum TaskStatus
    {
        [Description("Draft")]
        Pending = 1,
        [Description("Approved")]
        Approved = 2,
        [Description("On Going")]
        OnGoing = 3,
        //[Description("On Hold")]
        //OnHold = 4,
        //[Description("Canceled")]
        //Canceled = 5,
        [Description("Completed")]
        Completed = 4
    }
}
