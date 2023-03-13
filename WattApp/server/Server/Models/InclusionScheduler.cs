using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class InclusionScheduler
    {
        [ForeignKey(nameof(Device.Id))]
        public long DeviceId { get; set; }
        public string Day { get; set; }
        public TimeOnly TurnOn { get; set; }
        public TimeOnly TurnOff { get; set; }
        public string Comment { get; set; }
    }
}
