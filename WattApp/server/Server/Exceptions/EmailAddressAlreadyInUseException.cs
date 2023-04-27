namespace Server.Exceptions
{
    public class EmailAddressAlreadyInUseException : Exception
    {
        public EmailAddressAlreadyInUseException(String message) : base(message) { }
    }
}
