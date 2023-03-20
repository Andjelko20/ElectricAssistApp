using System.ComponentModel.DataAnnotations;

namespace Server.Models.DropDowns.Devices
{
    public class DeviceModel
    {
        [Key]
        public long Id { get; set; }
        public long DeviceTypeId { get; set; }
        public long DeviceBrandId { get; set; }
        public string Mark { get; set; }
        public float EnerguInKwh { get; set; }
        public float StandByKwh { get; set; }
    }
}
