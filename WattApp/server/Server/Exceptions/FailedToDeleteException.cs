namespace Server.Exceptions
{
    public class FailedToDeleteException : Exception
    {
        public FailedToDeleteException(String message) : base(message) { }
    }
}
