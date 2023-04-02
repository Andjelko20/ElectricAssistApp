using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Data;

namespace Server.Models
{
    public class ResetPasswordModel
    {
        [Key]
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public UserModel User { get; set; }

        [Required]
        public string ResetKey { get; set; }

        public DateTime ExpireAt { get; set; }=DateTime.Now.AddMinutes(5);


    }
}
