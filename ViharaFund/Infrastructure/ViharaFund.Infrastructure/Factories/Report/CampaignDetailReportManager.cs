using ViharaFund.Application.DTOs.Common;
using ViharaFund.Domain.Enums;
using ViharaFund.Infrastructure.Data;
using ViharaFund.Shared.DTOs.Report;

namespace ViharaFund.Infrastructure.Factories.Report
{
    public class CampaignDetailReportManager : ReportManager
    {
        public CampaignDetailReportManager(TenantDbContext dbContext) : base(dbContext)
        {
        }

        public override ReportType ReportType => ReportType.ActiveCampaignSummary;

        public override Task<FileDto> DownloadExcelReport(object parameters)
        {
            var reportParameters = (CampaignDetailReportFilter)parameters;
            throw new NotImplementedException();
        }

        public override Task<FileDto> DownloadPdfReport(object parameters)
        {
            var reportParameters = (CampaignDetailReportFilter)parameters;
            throw new NotImplementedException();
        }

        public override Task<PaginatedResultDTO<IReportResultDto>> GenerateReport(object parameters)
        {
            var reportParameters = (CampaignDetailReportFilter)parameters;
            throw new NotImplementedException();
        }
    }

    public class CampaignDetailReportFilter
    {

    }
}
