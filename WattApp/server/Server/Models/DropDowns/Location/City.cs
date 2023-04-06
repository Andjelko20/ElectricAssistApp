using Server.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.DropDowns.Location
{
    public class City
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(SqliteDbContext.Countries))]
        [Required]
        public long CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Country Country { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
