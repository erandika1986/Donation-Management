using Microsoft.AspNetCore.Http;

namespace ViharaFund.Application.DTOs.Common
{
    public class UploadFileDTO
    {
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
        public int JobCardTaskId { get; set; }
    }
}
