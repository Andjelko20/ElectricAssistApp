using Server.ValidationAtributes;
using System.ComponentModel.DataAnnotations;

namespace Server.DTOs
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

        [MinValue(1)]
        public int RoleId { get; set; } = 4;

        public UserCreateDTO()
        {

        }
    }
}
