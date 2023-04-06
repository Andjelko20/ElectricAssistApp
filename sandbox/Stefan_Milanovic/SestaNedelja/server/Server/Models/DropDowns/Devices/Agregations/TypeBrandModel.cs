using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.DropDowns.Devices.Agregations
{
    public class TypeBrandModel
    {
        [ForeignKey(nameof(DeviceType.Id))]
        public long TypeId { get; set; }
        [ForeignKey(nameof(DeviceBrand.Id))]
        public long BrandId { get; set; }
        [ForeignKey(nameof(DeviceModel.Id))]
        public long ModelId { get; set; }
        public float EnergyKwh { get; set; }
        public float StandByKwh { get; set; }
    }
}
