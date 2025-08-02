using ViharaFund.Domain.Entities.Common;
using ViharaFund.Domain.Enums;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class Campaign : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal TargetAmount { get; set; }
        public int CurrencyTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public bool HasEndDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CampaignCategoryId { get; set; }
        public string? CompaignImageUrl { get; set; }
        public CampaignVisibility Visibility { get; set; }
        public CampaignStatus Status { get; set; }

        public virtual CurrencyType CurrencyType { get; set; }
        public virtual CampaignCategory CampaignCategory { get; set; }

        public virtual ICollection<Donation> Donations { get; set; } = new HashSet<Donation>();
        public virtual ICollection<JobCard> JobCards { get; set; } = new HashSet<JobCard>();
    }
}
