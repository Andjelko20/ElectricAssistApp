using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Data;

namespace Server.Models.DropDowns.Devices
{
    public class DeviceModel
    {
        [Key]
        public long Id { get; set; }
        public string Mark { get; set; }
        public long DeviceTypeId { get; set; }
        [ForeignKey("DeviceTypeId")]
        public DeviceType DeviceType { get; set; }
        public long DeviceBrandId { get; set; }
        [ForeignKey("DeviceBrandId")]
        public DeviceBrand DeviceBrand { get; set; }
        public float EnergyKwh { get; set; }
        public float StandByKwh { get; set; }
    }
}
