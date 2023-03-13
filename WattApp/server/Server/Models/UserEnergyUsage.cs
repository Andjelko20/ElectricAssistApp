using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class UserEnergyUsage
    {
        [ForeignKey(nameof(UserModel.Id))]
        public long UserId { get; set; }
        public DateOnly Date { get; set; }
        public float Consumption { get; set; }
        public float Production { get; set; }
        public float EnergyStock { get; set; }
    }
}
