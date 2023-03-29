﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Server.Data;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryController : Controller
    {
        public readonly SqliteDbContext _sqliteDb;
        public readonly IHistoryService historyService;

        public HistoryController(SqliteDbContext sqliteDb, IHistoryService historyService)
        {
            _sqliteDb = sqliteDb;
            this.historyService = historyService;
        }

        /// <summary>
        /// Total device Consumption in last year
        /// </summary>
        [HttpGet]
        [Route("Year/Device/{deviceId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInLastYear([FromRoute] int deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId)) // Any - da li postoji stavka tog id-a u bazi
            {
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString()  + " does not exist." }); // ako ne postoji vrati NotFound
            }

            // ukoliko postoji vrati listu svih redova
            var historyList = historyService.GetUsageHistoryForDeviceInLastYear(deviceId);
            return Ok(historyList);
        }

        /// <summary>
        /// Total device Consumption in last month
        /// </summary>
        [HttpGet]
        [Route("Month/Device/{deviceId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInLastMonth([FromRoute] int deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
            {
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });
            }

            var historyList = historyService.GetUsageHistoryForDeviceInLastMonth(deviceId);
            return Ok(historyList);
        }

        /// <summary>
        /// Total device Consumption in last day
        /// </summary>
        [HttpGet]
        [Route("Day/Device/{deviceId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInLastDay([FromRoute] int deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceEnergyUsages.Any(u => u.DeviceId == deviceId))
                return Ok(0.0); // jer je potrosnja 0 ako nije paljen

            var HistoryForThePreviousDay = historyService.GetUsageHistoryForDeviceInLastDay(deviceId);
            return Ok(HistoryForThePreviousDay);
        }

        /// <summary>
        /// Total device Consumption in last week
        /// </summary>
        [HttpGet]
        [Route("Week/Device/{deviceId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInPastWeek([FromRoute] int deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceEnergyUsages.Any(u => u.DeviceId == deviceId))
                return Ok(0.0); // jer je potrosnja 0 ako nije paljen

            var HistoryForThePreviousDay = historyService.GetUsageHistoryForDeviceInPastWeek(deviceId);
            return Ok(HistoryForThePreviousDay);
        }

        /// <summary>
        /// Consumption history for device in past year by month
        /// </summary>
        [HttpGet]
        [Route("YearByMonth/Device/{deviceId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInPastYearByMonth([FromRoute] int deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });

            var HistoryForThePreviousYear = historyService.GetMonthlyEnergyUsage(deviceId);
            return Ok(HistoryForThePreviousYear);
        }

        /// <summary>
        /// Consumption history for device in past month by day
        /// </summary>
        [HttpGet]
        [Route("MonthByDay/Device/{deviceId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInPastMonthByDay([FromRoute] int deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });

            var HistoryForThePreviousMonth = historyService.GetDailyEnergyUsageForPastMonth(deviceId);
            return Ok(HistoryForThePreviousMonth);
        }

        /// <summary>
        /// Consumption history for device in past week by day
        /// </summary>
        [HttpGet]
        [Route("WeekByDay/Device/{deviceId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInPastWeekByDay([FromRoute] int deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });

            var HistoryForThePreviousMonth = historyService.GetDailyEnergyUsageForPastWeek(deviceId);
            return Ok(HistoryForThePreviousMonth);
        }

        /// <summary>
        /// Consumption for all user`s devices
        /// </summary>
        [HttpGet]
        [Route("TotalConsumption/User/{userId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetTotalConsumptionByUser([FromRoute] int userId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            var HistoryForTotalConsumption = historyService.GetTotalEnergyConsumptionForUser(userId);
            return Ok(HistoryForTotalConsumption);
        }

        /// <summary>
        /// Consumption for user, past day
        /// </summary>
        [HttpGet]
        [Route("Day/User/{userId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetConsumptionByUserForPastDay([FromRoute] int userId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            var HistoryForPastDayConsumption = historyService.GetUserEnergyConsumptionForPastDay(userId);
            return Ok(HistoryForPastDayConsumption);
        }

        /// <summary>
        /// Consumption for user in past week
        /// </summary>
        [HttpGet]
        [Route("Week/User/{userId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetConsumptionByUserForPastWeek([FromRoute] int userId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            var HistoryForPastWeekConsumption = historyService.GetUserEnergyConsumptionForPastWeek(userId);
            return Ok(HistoryForPastWeekConsumption);
        }

        /// <summary>
        /// Consumption for user in past month
        /// </summary>
        [HttpGet]
        [Route("Month/User/{userId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetConsumptionByUserForPastMonth([FromRoute] int userId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            var HistoryForPastMonthConsumption = historyService.GetUserEnergyConsumptionForPastMonth(userId);
            return Ok(HistoryForPastMonthConsumption);
        }

        /// <summary>
        /// Consumption for user in past year
        /// </summary>
        [HttpGet]
        [Route("Year/User/{userId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetConsumptionByUserForYear([FromRoute] int userId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            var HistoryForPastYearConsumption = historyService.GetUserEnergyConsumptionForPastYear(userId);
            return Ok(HistoryForPastYearConsumption);
        }

        /// <summary>
        /// Consumption for all user`s devices in past year (by month)
        /// </summary>
        [HttpGet]
        [Route("YearByMonth/User/{userId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetConsumptionByUserForYearByMonth([FromRoute] int userId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            if (!_sqliteDb.Devices.Any(u => u.UserId == userId))
                return Ok(0.0); // njemu je potrosnja 0 jer nema prijavljenih uredjaja

            var HistoryForPastYearByMonthConsumption = historyService.GetMonthlyEnergyUsageForPastYear(userId);
            return Ok(HistoryForPastYearByMonthConsumption);
        }
    }
}
