using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Price
    {
        [Key]
        public float PriceGreenZoneCheapPower { get; set; }
    }
}
