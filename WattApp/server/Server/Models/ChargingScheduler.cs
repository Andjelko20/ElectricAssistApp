using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class ChargingScheduler
    {
        [ForeignKey(nameof(Device.Id))]
        [Column(Order = 1)]
        public long Id { get; set; }
        [Key]
        [Column(Order = 1)] 
        public String day;
        [Key]
        [Column(Order = 2)]
        public String time;
        public String comment;
    }
}
