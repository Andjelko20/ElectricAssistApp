using Server.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Bill
    {
        [Required]
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public UserModel User { get; set; }
        [Required]
        public float Month { get; set; }
        [Required]
        public float Year { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float Consumption { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float Value { get; set; } //izracunava se na osnovu polja Consumption i podataka iz tabele Prices
    }
}
