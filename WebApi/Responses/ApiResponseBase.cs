namespace WebApi.Responses
{
    public abstract class ApiResponseBase
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        protected ApiResponseBase(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }

}
