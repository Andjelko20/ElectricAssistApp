using Server.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class InclusionScheduler
    {
        public long DeviceId { get; set; }
        [ForeignKey("DeviceId")]
        public Device Device { get; set; }
        [Required]
        public string Day { get; set; }
        [Required]
        public DateTime TurnOn { get; set; }
        [Required]
        public DateTime TurnOff { get; set; }
        public string? Comment { get; set; }
    }
}
