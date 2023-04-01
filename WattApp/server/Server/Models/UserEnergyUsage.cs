using Server.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class UserEnergyUsage
    {
        [ForeignKey(nameof(SqliteDbContext.Users))]
        public long UserId { get; set; }
        public DateOnly Date { get; set; }
        [Range(0, float.MaxValue)]
        public float? Consumption { get; set; }
        [Range(0, float.MaxValue)]
        public float? Production { get; set; }
        [Range(0, float.MaxValue)]
        public float? EnergyStock { get; set; }
    }
}
