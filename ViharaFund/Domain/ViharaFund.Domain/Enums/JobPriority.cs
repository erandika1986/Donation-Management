using System.ComponentModel;

namespace ViharaFund.Domain.Enums
{
    public enum JobPriority
    {
        [Description("Low")]
        Low = 1,
        [Description("Medium")]
        Medium = 2,
        [Description("High")]
        High = 3,
        [Description("Critical")]
        Critical = 4,
        [Description("Emergency")]
        Emergency = 5
    }
}
