using System.ComponentModel.DataAnnotations;

namespace Server.Models.DropDowns.Devices
{
    public class DeviceModel
    {
        //Frizider, TV, Bojler, Ostalo
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
