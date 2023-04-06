using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class ResetPasswordModel
    {
        [Key]
        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Required]
        public string ResetKey { get; set; }

        public DateTime ExpireAt { get; set; }=DateTime.Now.AddMinutes(5);


    }
}
