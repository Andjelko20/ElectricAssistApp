using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;

namespace Server.Models.DropDowns.Location
{
    public class Settlement
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(City.Id))]
        [Required]
        public long CityId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
