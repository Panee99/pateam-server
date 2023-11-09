namespace Application.Common.Exceptions
{
    public class UnauthorizedException : UnauthorizedAccessException
    {
        public UnauthorizedException()
        {
        }
        
        public UnauthorizedException(string message): base(message)
        {
        }
    }
}