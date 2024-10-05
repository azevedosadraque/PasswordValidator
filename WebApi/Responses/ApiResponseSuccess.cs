namespace WebApi.Responses
{
    public class ApiResponseSuccess<T> : ApiResponseBase
    {
        public T? Data { get; set; }

        public ApiResponseSuccess(T data, string? message = null)
            : base(true, message ?? "Request successful.")
        {
            Data = data;
        }
    }
}
