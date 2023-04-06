using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.DropDowns.Location
{
    public class City
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(Country.Id))]
        [Required]
        public long CountryId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
