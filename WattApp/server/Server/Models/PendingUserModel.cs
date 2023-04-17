﻿using Microsoft.EntityFrameworkCore;
using Server.Models.DropDowns.Location;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class PendingUserModel
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool Blocked { get; set; } = false;

        /*
        [ForeignKey(nameof(Country.Id))]
        public long? CountryId { get; set; }
        [ForeignKey(nameof(City.Id))]
        public long? CityId { get; set; }
        [ForeignKey(nameof(Settlement.Id))]
        
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        */
        public string Email { get; set; }

        public long SettlementId { get; set; }
        [ForeignKey("SettlementId")]
        public Settlement Settlement { get; set; }
        public string Address { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public long RoleId { get; set; }

        [ForeignKey("RoleId")]
        public RoleModel Role { get; set; }

        public DateTime ExpireAt { get; set; }
        public String ConfirmKey { get; set; }

    }
}
