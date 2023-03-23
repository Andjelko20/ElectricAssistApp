using System.ComponentModel.DataAnnotations;

namespace Server.Models.DropDowns.Devices
{
    public class DeviceModel
    {
        [Key]
        public long Id { get; set; }
        public string Mark { get; set; }
    }
}
