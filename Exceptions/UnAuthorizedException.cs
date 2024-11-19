using System.Net;

namespace RATAISHOP.Exceptions
{
    public class UnAuthorizedException
    {
        public class UnauthorizedException : CustomException
        {
            public List<string>? ErrorMessages { get; }

            public HttpStatusCode StatusCode { get; }

            public UnauthorizedException(string message, List<string>? errors = default, HttpStatusCode statusCode = HttpStatusCode.Unauthorized)
                : base(message)
            {
                ErrorMessages = errors;
                StatusCode = statusCode;
            }
        }
    }
}
