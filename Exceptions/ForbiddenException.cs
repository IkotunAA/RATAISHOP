using System.Net;

namespace RATAISHOP.Exceptions
{
    public class ForbiddenException : Exception
    {
        public List<string>? ErrorMessages { get; }

        public HttpStatusCode StatusCode { get; }

        public ForbiddenException(string message, List<string>? errors = default, HttpStatusCode statusCode = HttpStatusCode.Forbidden)
            : base(message)
        {
            ErrorMessages = errors;
            StatusCode = statusCode;
        }
    }
}
