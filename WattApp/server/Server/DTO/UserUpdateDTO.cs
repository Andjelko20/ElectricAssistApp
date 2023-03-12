using System.ComponentModel.DataAnnotations;

namespace Server.DTO
{
    public class UserUpdateDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Username { get; set; }

        public bool Blocked { get; set; }

        public int RoleId { get; set; }
    }
}
