using Server.Models.DropDowns.Devices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class DeviceDefaultSettings
    {
        [ForeignKey(nameof(DeviceModel.Id))]
        public long DeviceModelId { get; set; }
        [ForeignKey(nameof(DeviceBrand.Id))]
        public long DeviceBrandId { get; set; }
        [Range(0, float.MaxValue)]
        public float? DefaultKwh { get; set; }
    }
}
