namespace ViharaFund.Shared.DTOs.JobCard
{
    // Models
    //public class JobCardSummaryDTO1
    //{
    //    public int Id { get; set; }
    //    public string Title { get; set; } = string.Empty;
    //    public string Description { get; set; } = string.Empty;
    //    public JobCardStatus Status { get; set; }
    //    public string CreatedBy { get; set; } = string.Empty;
    //    public DateTime CreatedDate { get; set; }
    //    public string FirstApprover { get; set; } = string.Empty;
    //    public string SecondApprover { get; set; } = string.Empty;
    //    public decimal EstimatedBudget { get; set; }
    //    public decimal ActualCost { get; set; }
    //    public List<TaskModel> Tasks { get; set; } = new();
    //}

    public class TaskModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal EstimatedCost { get; set; }
        public decimal ActualCost { get; set; }
        public TaskStatus Status { get; set; }
        public List<ExpenseModel> Expenses { get; set; } = new();
    }

    public class ExpenseModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }

    //public enum JobCardStatus
    //{
    //    PendingFirstApproval,
    //    FirstApproved,
    //    FullyApproved,
    //    Rejected,
    //    Completed,
    //    Pending,
    //    InProgress
    //}

    //public enum TaskStatus
    //{
    //    Pending,
    //    InProgress,
    //    Completed
    //}
}
