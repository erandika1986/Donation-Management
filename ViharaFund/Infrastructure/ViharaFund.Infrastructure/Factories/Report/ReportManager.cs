using ViharaFund.Application.DTOs.Common;
using ViharaFund.Infrastructure.Data;
using ViharaFund.Shared.DTOs.Report;

namespace ViharaFund.Infrastructure.Factories.Report
{
    public abstract class ReportManager
    {
        internal readonly TenantDbContext DbContext;

        protected ReportManager(TenantDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "DbContext cannot be null");
        }

        public abstract Task<PaginatedResultDTO<IReportResultDto>> GenerateReport(BaseFilterDTO filters);

        public abstract Task<FileDto> DownloadExcelReport(BaseFilterDTO filters);

        public abstract Task<FileDto> DownloadPdfReport(BaseFilterDTO filter);
    }
}
