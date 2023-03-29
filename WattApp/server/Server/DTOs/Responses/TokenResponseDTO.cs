using System.ComponentModel.DataAnnotations;

namespace Server.DTOs.Responses
{
    public class TokenResponseDTO
    {
        [Required]
        public string token { get; set; }

        public TokenResponseDTO(string token)
        {
            this.token = token;
        }
    }
}
