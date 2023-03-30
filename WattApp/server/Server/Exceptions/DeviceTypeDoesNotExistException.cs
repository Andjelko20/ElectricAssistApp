namespace Server.Exceptions
{
    public class DeviceTypeDoesNotExistException : Exception
    {
        public DeviceTypeDoesNotExistException(String message) : base(message) { }
    }
}
