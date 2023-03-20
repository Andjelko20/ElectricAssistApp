using Server.Models.DropDowns.Devices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Device
    {
        [Key]
        public long Id { get; set; }
       
        [Required]
        [ForeignKey(nameof(UserModel.Id))]
        public long UserId { get; set; }
        [Required]
        [ForeignKey(nameof(DeviceCategory.Id))]
        public long DeviceCategoryId { get; set; } //from dropdown -> consumption, production, stock
        [Required]
        [ForeignKey(nameof(DeviceBrand.Id))]
        public long DeviceBrandId { get; set; } //from dropdown -> fox, vivax, tesla...
        [Required]
        [ForeignKey(nameof(DeviceType.Id))] 
        public long DeviceTypeId { get; set; } //from dropdown -> tv, car, other...
        [Required]
        [ForeignKey(nameof(DeviceModel.Id))]
        public long DeviceModelId { get; set; }
        public string? Name { get; set; }
        public float EnergyInKwh { get; set; }
        public float StandByKwh { get; set; }
        public bool Visibility { get; set; } = false;
        public bool Controlability { get; set; } = false;
        public bool TurnOn { get; set; } = false;

    }
}
