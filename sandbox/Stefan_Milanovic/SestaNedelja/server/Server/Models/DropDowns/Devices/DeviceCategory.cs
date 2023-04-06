using System.ComponentModel.DataAnnotations;

namespace Server.Models.DropDowns.Devices
{
    public class DeviceCategory
    {
        //Proizvodjac, potrosac, skladista
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
