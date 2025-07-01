namespace ViharaFund.Application.DTOs.Common
{
    public class FileDto
    {
        public MemoryStream FileContent { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
    }
}
