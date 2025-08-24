using ViharaFund.Domain.Enums;
using ViharaFund.Infrastructure.Data;

namespace ViharaFund.Infrastructure.Factories.Report
{
    public class ReportFactory
    {
        public static ReportManager GetReportManager(ReportType reportType, TenantDbContext tenantDbContext)
        {
            switch (reportType)
            {
                case ReportType.ActiveCampaignSummary:
                    return new CampaignDetailReportManager(tenantDbContext);
                case ReportType.CampaignJobReport:
                    return new CampaignJobDetailReportManager(tenantDbContext);
                case ReportType.CampaignTaskReport:
                    return new CampaignTaskDetailReportManager(tenantDbContext);
                default:
                    throw new NotImplementedException("Report type is not supported.");
            }
        }
    }
}
