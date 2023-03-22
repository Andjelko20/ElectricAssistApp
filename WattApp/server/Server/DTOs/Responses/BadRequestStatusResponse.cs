namespace Server.DTOs.Responses
{
    /// <summary>
    /// BadRequestResponse
    /// </summary>
    public class BadRequestStatusResponse : Dictionary<string, string[]>
    {
        public BadRequestStatusResponse() : base()
        {

        }

        public BadRequestStatusResponse(string message) : base()
        {
            this.Add("ERROR_MESSAGE", new string[] { message });
        }
    }
}
