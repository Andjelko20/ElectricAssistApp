using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Bill
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(UserModel.Id))]
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Month { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float Consumption { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float Value { get; set; } //izracunava se na osnovu pola Consumption i podataka iz tabele Prices
    }
}
