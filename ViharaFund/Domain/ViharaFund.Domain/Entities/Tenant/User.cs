using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class User : BaseAuditableEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        public virtual ICollection<JobCardApproval> JobCardApprovals { get; set; } = new HashSet<JobCardApproval>();
        public virtual ICollection<JobCardFundRequestApproval> JobCardFundRequestApprovals { get; set; } = new HashSet<JobCardFundRequestApproval>();
        public virtual ICollection<JobCardFundRequest> JobCardFundRequests { get; set; } = new HashSet<JobCardFundRequest>();
        public virtual ICollection<JobCardTaskPayment> JobCardTaskPayments { get; set; } = new HashSet<JobCardTaskPayment>();

        public virtual ICollection<User> CreatedUsers { get; set; } = new HashSet<User>();
        public virtual ICollection<User> UpdatedUsers { get; set; } = new HashSet<User>();

        public virtual ICollection<Campaign> CreatedCampaigns { get; set; } = new HashSet<Campaign>();
        public virtual ICollection<Campaign> UpdatedCampaigns { get; set; } = new HashSet<Campaign>();

        public virtual ICollection<Donation> CreatedDonations { get; set; } = new HashSet<Donation>();
        public virtual ICollection<Donation> UpdatedDonations { get; set; } = new HashSet<Donation>();

        public virtual ICollection<DonationExpense> CreatedDonationExpenses { get; set; } = new HashSet<DonationExpense>();
        public virtual ICollection<DonationExpense> UpdatedDonationExpenses { get; set; } = new HashSet<DonationExpense>();

        public virtual ICollection<Donor> CreatedDonors { get; set; } = new HashSet<Donor>();
        public virtual ICollection<Donor> UpdatedDonors { get; set; } = new HashSet<Donor>();

        public virtual ICollection<JobCard> CreatedJobCards { get; set; } = new HashSet<JobCard>();
        public virtual ICollection<JobCard> UpdatedJobCards { get; set; } = new HashSet<JobCard>();

        public virtual ICollection<JobCardComment> CreatedJobCardComments { get; set; } = new HashSet<JobCardComment>();
        public virtual ICollection<JobCardComment> UpdatedJobCardComments { get; set; } = new HashSet<JobCardComment>();

        public virtual ICollection<JobCardHistory> CreatedJobCardHistories { get; set; } = new HashSet<JobCardHistory>();
        public virtual ICollection<JobCardHistory> UpdatedJobCardHistories { get; set; } = new HashSet<JobCardHistory>();

        public virtual ICollection<JobCardApproval> CreatedJobCardApprovals { get; set; } = new HashSet<JobCardApproval>();
        public virtual ICollection<JobCardApproval> UpdatedJobCardApprovals { get; set; } = new HashSet<JobCardApproval>();

        public virtual ICollection<JobCardFundRequestComment> CreatedJobCardFundRequestComments { get; set; } = new HashSet<JobCardFundRequestComment>();
        public virtual ICollection<JobCardFundRequestComment> UpdatedJobCardFundRequestComments { get; set; } = new HashSet<JobCardFundRequestComment>();

        public virtual ICollection<JobCardFundRequest> CreatedJobCardFundRequests { get; set; } = new HashSet<JobCardFundRequest>();
        public virtual ICollection<JobCardFundRequest> UpdatedJobCardFundRequests { get; set; } = new HashSet<JobCardFundRequest>();

        public virtual ICollection<JobCardFundRequestRelease> CreatedJobCardFundRequestReleases { get; set; } = new HashSet<JobCardFundRequestRelease>();
        public virtual ICollection<JobCardFundRequestRelease> UpdatedJobCardFundRequestReleases { get; set; } = new HashSet<JobCardFundRequestRelease>();

        public virtual ICollection<JobCardFundRequestApproval> CreatedJobCardFundRequestApprovals { get; set; } = new HashSet<JobCardFundRequestApproval>();
        public virtual ICollection<JobCardFundRequestApproval> UpdatedJobCardFundRequestApprovals { get; set; } = new HashSet<JobCardFundRequestApproval>();

        public virtual ICollection<JobCardTask> CreatedJobCardTasks { get; set; } = new HashSet<JobCardTask>();
        public virtual ICollection<JobCardTask> UpdatedJobCardTasks { get; set; } = new HashSet<JobCardTask>();

        public virtual ICollection<JobCardTaskComment> CreatedJobCardTaskComments { get; set; } = new HashSet<JobCardTaskComment>();
        public virtual ICollection<JobCardTaskComment> UpdatedJobCardTaskComments { get; set; } = new HashSet<JobCardTaskComment>();

        public virtual ICollection<JobCardTaskAttachment> CreatedJobCardTaskAttachments { get; set; } = new HashSet<JobCardTaskAttachment>();
        public virtual ICollection<JobCardTaskAttachment> UpdatedJobCardTaskAttachments { get; set; } = new HashSet<JobCardTaskAttachment>();

        public virtual ICollection<JobCardTaskPayment> CreatedJobCardTaskPayments { get; set; } = new HashSet<JobCardTaskPayment>();
        public virtual ICollection<JobCardTaskPayment> UpdatedJobCardTaskPayments { get; set; } = new HashSet<JobCardTaskPayment>();
    }
}
