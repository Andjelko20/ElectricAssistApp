using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Data;

namespace Server.Models.DropDowns.Location
{
    public class Settlement
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long CityId { get; set; }
        [ForeignKey("CityId")]
        public City City { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
