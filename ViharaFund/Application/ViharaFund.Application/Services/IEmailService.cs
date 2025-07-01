using ViharaFund.Application.DTOs.Common;

namespace ViharaFund.Application.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string htmlBody);
        Task SendEmailWithAttachmentsAsync(string to, string subject, string htmlBody, List<EmailAttachmentDTO> attachments);
        Task SendEmailToMultipleRecipientsAsync(List<string> toAddresses, List<string> ccList, string subject, string htmlBody, List<EmailAttachmentDTO> attachments = null);
    }
}
