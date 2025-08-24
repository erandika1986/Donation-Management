using System.ComponentModel;

namespace ViharaFund.Domain.Enums
{
    public enum ReportType
    {
        [Description("Active Campaign Summary")]
        ActiveCampaignSummary = 1,

        [Description("Campaign Job Report")]
        CampaignJobReport = 2,

        [Description("Campaign Task Report")]
        CampaignTaskReport = 3,
    }
}
