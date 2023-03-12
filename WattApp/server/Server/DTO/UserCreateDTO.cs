using Server.ValidationAtributes;
using System.ComponentModel.DataAnnotations;

namespace Server.DTO
{
    public class UserCreateDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool Blocked { get; set; }

        [Required]
        [MinValue(1)]
        public int RoleId { get; set; }

        public UserCreateDTO()
        {

        }
    }
}
