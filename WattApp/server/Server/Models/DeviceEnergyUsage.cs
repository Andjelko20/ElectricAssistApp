using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Data;

namespace Server.Models
{
    public class DeviceEnergyUsage
    {
        public long DeviceId { get; set; }
        [ForeignKey("DeviceId")]
        public Device Device { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [Range(0, float.MaxValue)]
        public float EnergyInKwh { get; set; }
    }
}
