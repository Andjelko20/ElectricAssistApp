using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Data;

namespace Server.Models
{
    public class ResetPasswordModel
    {
        [Key]
        [ForeignKey(nameof(SqliteDbContext.Users))]
        public long UserId { get; set; }

        [Required]
        public string ResetKey { get; set; }

        public DateTime ExpireAt { get; set; }=DateTime.Now.AddMinutes(5);


    }
}
