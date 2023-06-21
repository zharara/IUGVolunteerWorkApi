using System.Net;

namespace VolunteerWorkApi.Helpers.ErrorHandling
{
    public class ApiResponseException : Exception
    {
        public ApiResponseException(
            HttpStatusCode statusCode,
            string errorTitle, string errorMessage,
            string? errorDetails = null) =>
            (StatusCode, ErrorTitle, ErrorMessage, ErrorDetails) =
            (statusCode, errorTitle, errorMessage, errorDetails);

        public HttpStatusCode StatusCode { get; }
        public string ErrorTitle { get; }
        public string ErrorMessage { get; }
        public string? ErrorDetails { get; }
    }
}
