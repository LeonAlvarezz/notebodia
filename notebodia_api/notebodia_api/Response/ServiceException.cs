namespace notebodia_api.Response
{
    public class ServiceException(int statusCode, string message) : Exception
    {
        public int StatusCode { get; } = statusCode;
        public string ErrorMessage { get; } = message;
    }

    public class UnauthorizedException : ServiceException
    {
        public UnauthorizedException(string message = "Unauthorized")
            : base(StatusCodes.Status401Unauthorized, message)
        {
        }
    }

    public class NotFoundException : ServiceException
    {
        public NotFoundException(string message = "Not Found")
            : base(StatusCodes.Status404NotFound, message)
        {
        }
    }
    public class InternalServerException : ServiceException
    {
        public InternalServerException(string message = "Something Went Wrong")
            : base(StatusCodes.Status500InternalServerError, message)
        {
        }
    }

    public class ForbiddenException : ServiceException
    {
        public ForbiddenException(string message = "Forbidden")
            : base(StatusCodes.Status403Forbidden, message)
        {
        }
    }

    public class DuplicateResourceException : ServiceException
    {
        public DuplicateResourceException(string message = "Duplicate Resource")
            : base(StatusCodes.Status409Conflict, message)
        {
        }
    }

}
