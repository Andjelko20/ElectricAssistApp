namespace Server.DTOs
{
    public class ActionFailedDTO
    {
        public string inputFieldName { get; set; }
        public string message { get; set; }

        public ActionFailedDTO(string inputFieldName, string message)
        {
            this.inputFieldName = inputFieldName;
            this.message = message;
        }
    }
}
