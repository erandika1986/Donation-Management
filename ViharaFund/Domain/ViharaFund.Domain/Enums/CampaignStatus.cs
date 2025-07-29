using System.ComponentModel;

namespace ViharaFund.Domain.Enums
{
    public enum CampaignStatus
    {
        [Description("Draft")]
        Draft = 1,
        [Description("Active")]
        Active = 2,
        [Description("Completed")]
        Completed = 3,
        [Description("Paused")]
        Paused = 4
    }
}
