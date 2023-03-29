using System.ComponentModel.DataAnnotations;

namespace Server.DTOs.Requests
{
    public class EmailRequestDTO
    {
        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Not email")]
        public string Email { get; set; }
    }
}
