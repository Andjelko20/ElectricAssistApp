namespace Server.Exceptions
{
    public class UserNotAuthorizedException : Exception
    {
        public UserNotAuthorizedException(String message) : base(message) { }
    }
}
