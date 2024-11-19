using System.Net;

namespace RATAISHOP.Exceptions
{
    public class CustomException : Exception
    {
        public List<string>? ErrorMessages { get; }

        public HttpStatusCode StatusCode { get; }

        public CustomException(string message, List<string>? errors = default, HttpStatusCode statusCode = HttpStatusCode.Forbidden)
            : base(message)
        {
            ErrorMessages = errors;
            StatusCode = statusCode;
        }
    }
}
