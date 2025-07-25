namespace ViharaFund.Admin.Common
{
    public class ResponseDto
    {
        public int Id { get; set; }
        public bool Succeeded { get; set; }
        public string SuccessMessage { get; set; }
        public string Data { get; set; }
        public string[] Errors { get; set; }
    }
}
