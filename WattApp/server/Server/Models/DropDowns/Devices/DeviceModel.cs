﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Data;

namespace Server.Models.DropDowns.Devices
{
    public class DeviceModel
    {
        [Key]
        public long Id { get; set; }
        public string Mark { get; set; }

        [ForeignKey(nameof(SqliteDbContext.DeviceTypes))]
        public long DeviceTypeId { get; set; }
    }
}
