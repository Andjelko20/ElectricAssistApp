using System.ComponentModel.DataAnnotations;

namespace Server.DTOs
{
    public class BlockedStatusDTO
    {
        [Required]
        public bool Status { get; set; }
    }
}
