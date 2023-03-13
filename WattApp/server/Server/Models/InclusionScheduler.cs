using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class InclusionScheduler
    {
        [ForeignKey(nameof(Device.Id))] 
        public long Id { get; set; }
        public DateTime TurnOn { get; set; }
        public DateTime TurnOff { get; set; }
        public string Comment { get; set; }
    }
}
