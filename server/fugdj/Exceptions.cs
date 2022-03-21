namespace fugdj
{
    public abstract class HttpException : SystemException
    {
        public int StatusCode;

        protected HttpException(string message) : base(message)
        {
        }
    }
    
    public class ResourceNotFoundException : HttpException
    {
        public ResourceNotFoundException() : base("404: Resource Not Found")
        {
            StatusCode = 404;
        }
        
        public ResourceNotFoundException(string message) : base(message)
        {
            StatusCode = 404;
        }
    }

    public class BadRequestException : HttpException
    {
        public BadRequestException() : base("400: Bad Request")
        {
            StatusCode = 400;
        }
        
        public BadRequestException(string message) : base(message)
        {
            StatusCode = 400;
        }
    }
    
    public class UnauthorisedException : HttpException
    {
        public UnauthorisedException() : base("401: Unauthorised")
        {
            StatusCode = 401;
        }
    }
    
    public class InternalServerException : HttpException
    {
        public InternalServerException() : base("500: Internal Server Error")
        {
            StatusCode = 500;
        }
    }
}