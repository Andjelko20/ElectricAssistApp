using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class DeviceEnergyUsage
    {
        public long DeviceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [Range(0, float.MaxValue)]
        public float EnergyInKWh { get; set; }
    }
}
