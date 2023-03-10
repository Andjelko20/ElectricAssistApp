using System.ComponentModel.DataAnnotations;

namespace Server.DTO
{
    public class BlockedStatusDTO
    {
        [Required]
        public bool Status { get; set; }
    }
}
