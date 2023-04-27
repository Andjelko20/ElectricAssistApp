using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class ChangeEmailModel
    {
        [Key]
        public long UserId { get; set; }
        public string OldEmail { get; set; }
        public string NewEmail { get; set; }
        public DateTime ExpireAt { get; set; } = DateTime.Now.AddDays(1);
        public string ChangeEmailKey { get; set; }
    }
}
