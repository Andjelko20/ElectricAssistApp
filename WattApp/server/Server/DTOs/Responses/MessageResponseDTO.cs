namespace Server.DTOs.Responses
{
    public class MessageResponseDTO
    {
        public string message { get; set; }
        public MessageResponseDTO(string message)
        {
            this.message = message;
        }
    }
}
