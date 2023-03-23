using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
