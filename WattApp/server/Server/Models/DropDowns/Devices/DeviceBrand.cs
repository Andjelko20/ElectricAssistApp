using System.ComponentModel.DataAnnotations;

namespace Server.Models.DropDowns.Devices
{
    public class DeviceBrand
    {
        //Vox, Vivax, Tesla...
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
