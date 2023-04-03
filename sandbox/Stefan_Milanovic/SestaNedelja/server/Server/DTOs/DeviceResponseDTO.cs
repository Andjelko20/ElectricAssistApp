using Microsoft.AspNetCore.Routing.Constraints;
using System.Globalization;

namespace Server.DTOs
{
    public class DeviceResponseDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string DeviceCategory { get; set; }
        public string DeviceType { get; set; }
        public string DeviceBrand { get;  set; }
        public string DeviceModel { get; set; }
        public string Name { get; private set; }
        public float EnergyInKwh { get; set; }
        public float StandByKwh { get; set; }
        public bool Visibility { get; set; }
        public bool Controlability { get; set; }
        public bool TurnOn { get; set; }
    }
}
