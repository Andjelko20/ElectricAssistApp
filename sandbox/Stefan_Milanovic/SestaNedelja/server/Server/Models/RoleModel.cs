using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class RoleModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
