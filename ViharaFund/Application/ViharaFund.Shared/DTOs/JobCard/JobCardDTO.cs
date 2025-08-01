using ViharaFund.Application.DTOs.Common;

namespace ViharaFund.Application.DTOs.JobCard
{
    public class JobCardDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DropDownDTO Priority { get; set; }
        public DropDownDTO Status { get; set; }
        public DropDownDTO AssignedRoleGroup { get; set; }
        public decimal? EstimatedTotalAmount { get; set; }
        public decimal? ActualTotalAmount { get; set; }
        public string? AdditionalNote { get; set; }
    }
}
