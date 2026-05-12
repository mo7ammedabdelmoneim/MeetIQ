namespace MeetIQ.Application.Common.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "You do not have permission to perform this action.")
            : base(message) { }
    }
}
