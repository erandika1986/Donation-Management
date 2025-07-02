using System.ComponentModel;

namespace ViharaFund.Domain.Enums
{
    public enum ApprovalLevel
    {
        [Description("One Level")]
        OneLevel = 1,
        [Description("Two Levels")]
        TwoLevel = 2,
        [Description("Three Levels")]
        ThreeLevel = 3,
    }
}
