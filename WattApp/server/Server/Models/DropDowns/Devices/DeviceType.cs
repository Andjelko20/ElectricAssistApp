using Server.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.DropDowns.Devices
{
    public class DeviceType
    {
        //Frizider, TV, Bojler, Ostalo
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(SqliteDbContext.DeviceCategories))]
        public long CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
