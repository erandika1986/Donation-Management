using Microsoft.AspNetCore.Mvc;
using ViharaFund.Infrastructure.Data;
using ViharaFund.Infrastructure.Factories.Report;
using ViharaFund.Shared.DTOs.Report;

namespace ViharaFund.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly TenantDbContext tenantDbContext;
        public ReportController(TenantDbContext tenantDbContext)
        {
            this.tenantDbContext = tenantDbContext;
        }


        [HttpPost("generateReport")]
        public async Task<IActionResult> GenerateReport([FromBody] ReportParamDTO filters)
        {
            var manager = ReportFactory.GetReportManager(filters.ReportType, tenantDbContext);
            var report = manager.GenerateReport(filters.Parameters);
            return Ok(report);
        }


        [HttpPost("downloadPdfReport")]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<FileStreamResult> DownloadPdfReport([FromBody] ReportParamDTO filters)
        {
            var manager = ReportFactory.GetReportManager(filters.ReportType, tenantDbContext);

            var response = await manager.DownloadPdfReport(filters.Parameters);


            byte[] pdfFile = response.FileContent.ToArray();

            return File(new MemoryStream(pdfFile), response.MimeType, response.FileName);

        }


        [HttpPost("downloadExcelReport")]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<FileStreamResult> DownloadExcelReport([FromBody] ReportParamDTO filters)
        {
            var manager = ReportFactory.GetReportManager(filters.ReportType, tenantDbContext);

            var response = await manager.DownloadPdfReport(filters.Parameters);

            byte[] pdfFile = response.FileContent.ToArray();

            return File(new MemoryStream(pdfFile), response.MimeType, response.FileName);
        }
    }
}
