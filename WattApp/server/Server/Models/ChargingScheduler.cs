using Server.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class ChargingScheduler
    {
        public long DeviceId { get; set; }
        [ForeignKey("DeviceId")]
        public Device Device{get;set;}
        public String Day { get; set; }
        public String Time { get; set; }
        public String? Comment { get; set; }
    }
}
