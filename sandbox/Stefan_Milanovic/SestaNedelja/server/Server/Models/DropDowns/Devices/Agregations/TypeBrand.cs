using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.DropDowns.Devices.Agregations
{
    public class TypeBrand
    {
        [ForeignKey(nameof(DeviceType.Id))]
        public long TypeId { get; set; }
        [ForeignKey(nameof(DeviceBrand.Id))]
        public long BrandId { get; set; }
    }
}
