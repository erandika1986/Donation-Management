using ViharaFund.Domain.Entities.Common;
using ViharaFund.Domain.Enums;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCardFundRequestApproval : BaseAuditableEntity
    {
        public int FundRequestId { get; set; }
        public int ApproverId { get; set; }
        public ApprovalStatus Status { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? Comment { get; set; }


        public virtual JobCardFundRequest JobCardFundRequest { get; set; }
        public virtual User Approver { get; set; }
    }
}
