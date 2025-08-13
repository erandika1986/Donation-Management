using ViharaFund.Application.DTOs.Common;
using ViharaFund.Infrastructure.Data;
using ViharaFund.Shared.DTOs.Report;

namespace ViharaFund.Infrastructure.Factories.Report
{
    public class CampaignJobDetailReportManager : ReportManager
    {
        public CampaignJobDetailReportManager(TenantDbContext dbContext) : base(dbContext)
        {
        }
        public override Task<FileDto> DownloadExcelReport(BaseFilterDTO filters)
        {
            throw new NotImplementedException();
        }
        public override Task<FileDto> DownloadPdfReport(BaseFilterDTO filter)
        {
            throw new NotImplementedException();
        }
        public override Task<PaginatedResultDTO<IReportResultDto>> GenerateReport(BaseFilterDTO filters)
        {
            throw new NotImplementedException();
        }
    }
}
