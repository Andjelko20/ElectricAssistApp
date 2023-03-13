using Microsoft.AspNetCore.Routing.Constraints;

namespace Server.Models
{
    public class DeviceEnergyUsage
    {
        public long DeviceId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public float KWh { get; set; }
    }
}
