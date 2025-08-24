using ViharaFund.Domain.Enums;

namespace ViharaFund.Shared.DTOs.Report
{
    public class ReportParamDTO
    {
        public ReportType ReportType { get; set; }
        public object Parameters { get; set; }
    }
}
