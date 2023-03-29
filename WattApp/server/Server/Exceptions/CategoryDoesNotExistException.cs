namespace Server.Exceptions
{
    public class CategoryDoesNotExistException : Exception
    {
        public CategoryDoesNotExistException(String message) : base(message) { }
    }
}
