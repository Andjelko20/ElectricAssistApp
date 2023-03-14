using Microsoft.EntityFrameworkCore;
using Server.Models.DropDowns.Location;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(RoleModel.Id))]
        public int RoleId { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool Blocked { get; set; } = false;

        [ForeignKey(nameof(Country.Id))]
        public long? CountryId { get; set; }
        [ForeignKey(nameof(City.Id))]
        public long? CityId { get; set; }
        [ForeignKey(nameof(Settlement.Id))]
        public long? SettlementId { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }



    }
}
