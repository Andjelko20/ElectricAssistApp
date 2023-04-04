﻿using System.ComponentModel.DataAnnotations;

namespace Server.DTOs
{
    public class UpdateUserByAdminDTO
    {
        public bool Blocked { get; set; }

        public long RoleId { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
