using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class ChangeEmailModel
    {
        [Key]
        public long UserId { get; set; }
        public String OldEmail { get; set; }
        public String NewEmail { get; set; }
        public DateTime ExpireAt { get; set; } = DateTime.Now.AddDays(1);
        public String ChangeEmailKey { get; set; }
    }
}
