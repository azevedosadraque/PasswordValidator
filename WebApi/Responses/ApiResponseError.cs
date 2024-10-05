namespace WebApi.Responses
{
    public class ApiResponseError : ApiResponseBase
    {
        public List<string>? Errors { get; set; }

        public ApiResponseError(string message, List<string> errors)
            : base(false, message)
        {
            Errors = errors;
        }
    }

}
