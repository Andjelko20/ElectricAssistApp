using System.ComponentModel.DataAnnotations;

namespace Server.DTOs.Responses
{
    public class TokenResponseDTO
    {
        [Required]
        public string Email { get; set; }
    }
}
