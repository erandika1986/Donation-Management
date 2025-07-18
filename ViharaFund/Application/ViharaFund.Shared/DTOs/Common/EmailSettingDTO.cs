namespace ViharaFund.Application.DTOs.Common
{
    public class EmailSetting
    {
        public string SMTPServer { get; set; }
        public string SMTPPort { get; set; }
        public string SMTPUsername { get; set; }
        public string SMTPPassword { get; set; }
        public string SMTPAuth { get; set; }
        public string SMTPTLS { get; set; }
        public string FromEmail { get; set; }
        public string SMTPUseDefaultCredentials { get; set; }
        public string SMTPEnableSsl { get; set; }
    }
}
