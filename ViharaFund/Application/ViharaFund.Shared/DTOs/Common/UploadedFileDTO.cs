namespace ViharaFund.Shared.DTOs.Common
{
    public class UploadedFileDTO
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string PreviewUrl { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
    }
}
