using System.ComponentModel.DataAnnotations;

namespace Server.Models.DropDowns.Location
{
    public class Country
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
