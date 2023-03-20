using Microsoft.AspNetCore.Routing.Constraints;
using System.Globalization;

namespace Server.DTOs
{
    public class DeviceResponseDTO
    {
        public long UserId { get; private set; }
        public string DeviceCategory { get; private set; }
        public string DeviceType { get; private set; }
        public string DeviceBrand { get; private set; }
        public string DeviceModel { get; private set; }
        public string Name { get; private set; }
        public float EnergyInKwh { get; private set; }
        public float StandByKwh { get; private set; }
        public bool Visibility { get; private set; }
        public bool Controlability { get; private set; }
        public bool TurnOn { get; private set; }
    }
}
