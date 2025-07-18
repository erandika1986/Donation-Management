namespace ViharaFund.Application.DTOs.Common
{
    public class ResultDto
    {
        internal ResultDto(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        internal ResultDto(bool succeeded, string successMessage, int id = 0, string data = null)
        {
            Succeeded = succeeded;
            SuccessMessage = successMessage;
            Id = id;
            Data = data;
        }

        public int Id { get; set; }
        public bool Succeeded { get; set; }
        public string SuccessMessage { get; set; }
        public string Data { get; set; }
        public string[] Errors { get; set; }

        public static ResultDto Success(string messeage, int id = 0, string data = null)
        {
            return new ResultDto(true, messeage, id, data);
        }

        public static ResultDto Failure(IEnumerable<string> errors)
        {
            return new ResultDto(false, errors);
        }
    }
}
