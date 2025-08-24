using ViharaFund.Application.DTOs.Common;
using ViharaFund.Domain.Enums;
using ViharaFund.Infrastructure.Data;
using ViharaFund.Shared.DTOs.Report;

namespace ViharaFund.Infrastructure.Factories.Report
{
    public class CampaignJobDetailReportManager : ReportManager
    {
        public CampaignJobDetailReportManager(TenantDbContext dbContext) : base(dbContext)
        {
        }

        public override ReportType ReportType => ReportType.CampaignJobReport;

        public override Task<FileDto> DownloadExcelReport(object parameters)
        {
            var reportParameters = (CampaignJobDetailReportFilters)parameters;
            throw new NotImplementedException();
        }
        public override Task<FileDto> DownloadPdfReport(object parameters)
        {
            var reportParameters = (CampaignJobDetailReportFilters)parameters;
            throw new NotImplementedException();
        }
        public override Task<PaginatedResultDTO<IReportResultDto>> GenerateReport(object parameters)
        {
            var reportParameters = (CampaignJobDetailReportFilters)parameters;
            throw new NotImplementedException();
        }
    }

    public class CampaignJobDetailReportFilters
    {

    }
}
