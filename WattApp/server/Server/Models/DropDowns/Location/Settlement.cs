using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Data;

namespace Server.Models.DropDowns.Location
{
    public class Settlement
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(SqliteDbContext.Cities))]
        [Required]
        public long CityId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
