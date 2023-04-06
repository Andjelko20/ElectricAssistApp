using System.ComponentModel.DataAnnotations;

namespace Server.DTOs.Requests
{
    public class ResetPasswordRequestDTO
    {
        [Required]
        public string ResetKey { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
