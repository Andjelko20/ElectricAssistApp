using Server.Data;
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
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public UserModel User { get; set; }
        //[Required]
        //[ForeignKey(nameof(SqliteDbContext.DeviceCategories))]
        //public long DeviceCategoryId { get; set; } //from dropdown -> consumption, production, stock
        //[Required]
        //[ForeignKey(nameof(DeviceBrand.Id))]
        //public long DeviceBrandId { get; set; } //from dropdown -> fox, vivax, tesla...
        //[Required]
        //[ForeignKey(nameof(DeviceType.Id))] 
        //public long DeviceTypeId { get; set; } //from dropdown -> tv, car, other...
        [Required]
        public long DeviceModelId { get; set; }
        [ForeignKey("DeviceModelId")]
        public DeviceModel DeviceModel { get; set; }
        public string? Name { get; set; }
        public float EnergyInKwh { get; set; }
        public float StandByKwh { get; set; }
        public bool Visibility { get; set; } = false;
        public bool Controlability { get; set; } = false;
        public bool TurnOn { get; set; } = false;

    }
}
