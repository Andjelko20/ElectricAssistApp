using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    [Index(nameof(UserModel.Username),IsUnique = true)]
    public class UserModel
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }
        //[Required]
        //public string LastName { get; set; }

        [Required]
        public string Username { get; set; } 

        [Required]
        public string Password { get; set; }

        [Required]
        [ForeignKey(nameof(RoleModel.Id))]
        public int RoleId { get; set; }

        [Required]
        public bool Blocked { get; set; }

        public string? Address { get; set; }
        public long CountryId { get; set; } 
        public long CityId { get; set; }
        public string? PhoneNumeber { get; set; }
        public string? Email { get; set; }



    }
}
