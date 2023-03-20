using Microsoft.EntityFrameworkCore;
using Server.Models.DropDowns.Location;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Server.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool Blocked { get; set; } = false;

        /*
        [ForeignKey(nameof(Country.Id))]
        public long? CountryId { get; set; }
        [ForeignKey(nameof(City.Id))]
        public long? CityId { get; set; }
        [ForeignKey(nameof(Settlement.Id))]
        public long? SettlementId { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        */

        public string Email { get; set; }

        [ForeignKey(nameof(RoleModel.Id))]
        public int RoleId { get; set; }

        public RoleModel? Role { get; set; }
    }
}
