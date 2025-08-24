using ViharaFund.Application.DTOs.Common;
using ViharaFund.Domain.Enums;
using ViharaFund.Infrastructure.Data;
using ViharaFund.Shared.DTOs.Report;

namespace ViharaFund.Infrastructure.Factories.Report
{
    public abstract class ReportManager
    {
        internal readonly TenantDbContext DbContext;

        public abstract ReportType ReportType { get; }

        protected ReportManager(TenantDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "DbContext cannot be null");
        }

        public abstract Task<PaginatedResultDTO<IReportResultDto>> GenerateReport(object parameters);

        public abstract Task<FileDto> DownloadExcelReport(object parameters);

        public abstract Task<FileDto> DownloadPdfReport(object parameters);
    }
}
