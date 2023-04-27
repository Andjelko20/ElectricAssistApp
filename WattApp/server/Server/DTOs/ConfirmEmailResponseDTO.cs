namespace Server.DTOs
{
    public class ConfirmEmailResponseDTO
    {
        public Boolean isConfirmed { get; set; } = false;
        public String error { get; set; } = null;
    }
}
