namespace ViharaFund.Application.DTOs.Common
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            ToEmails = new List<string>();
            Attachements = new List<FileDto>();
        }
        public string Subject { get; set; }
        public string EmailBody { get; set; }
        public string FromEmail { get; set; }
        public List<string> ToEmails { get; set; }
        public List<FileDto> Attachements { get; set; }
        public bool IsHtmlEnable { get; set; }
        public string HTMLTemplatePath { get; set; }

        //for queue email
        public bool? IsQueueEmail { get; set; }
        public int? FailedEmailId { get; set; }
    }
}
