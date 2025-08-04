using ViharaFund.Domain.Enums;

namespace ViharaFund.Application.DTOs.JobCard
{
    public class JobCardApprovalDTO
    {
        public int Id { get; set; }
        public int JobCardId { get; set; }
        public string ApproveLevelName { get; set; }
        public int ApprovalLevelId { get; set; }
        public string ApproverUser { get; set; }
        public string ApprovedDate { get; set; }
        public ApprovalStatus Status { get; set; }
        public string? Remarks { get; set; }
    }
}
