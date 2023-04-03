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

        [Required]
        public string Email { get; set; }

        [MinValue(1)]
        public long RoleId { get; set; } = 4;

        public long SettlementId { get; set; }
        public string Address { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public UserCreateDTO()
        {

        }
    }
}
