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
        [ForeignKey(nameof(DeviceType.Id))]
        public long DeviceTypeId { get; set; } //from dropdown -> consumption, production, stock
        [Required]
        [ForeignKey(nameof(UserModel.Id))]
        public long UserId { get; set; }
        public string DeviceBrand { get; set; } = string.Empty;
        [Required]
        [ForeignKey(nameof(DeviceModel.Id))] 
        public long DeviceModelId { get; set; } //from dropdown -> tv, car, other...
        public string Name { get; set; } = string.Empty;
        public float EnergyInKwh { get; set; }
        public bool Visibility { get; set; } = false;
        public bool Controlability { get; set; } = false;
        public bool TurnOn { get; set; } = false;

    }
}
