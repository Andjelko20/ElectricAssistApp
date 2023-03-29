using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Server.Data;
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
    }
}
