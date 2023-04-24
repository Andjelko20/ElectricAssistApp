using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrentPeriodHistoryController : Controller
    {
        public readonly SqliteDbContext _sqliteDb;
        public readonly ICurrentPeriodHistoryService currentPeriodHistoryService;

        public CurrentPeriodHistoryController(SqliteDbContext sqliteDb, ICurrentPeriodHistoryService currentPeriodHistoryService)
        {
            _sqliteDb = sqliteDb;
            this.currentPeriodHistoryService = currentPeriodHistoryService;
        }

        /// <summary>
        /// Total device Consumption/Production for current year, month, day
        /// </summary>
        [HttpGet]
        [Route("device")]
        public async Task<IActionResult> GetHistoryForDeviceFromCurrentYear([FromQuery] long doubleYearDeviceId, long doubleMonthDeviceId, long doubleDayDeviceId)
        {
            if (doubleYearDeviceId != 0)
            {
                if (!_sqliteDb.Devices.Any(u => u.Id == doubleYearDeviceId))
                    return NotFound(new { message = "Device with the ID: " + doubleYearDeviceId.ToString() + " does not exist." });

                var result = currentPeriodHistoryService.GetUsageHistoryForDeviceFromCurrentYear(doubleYearDeviceId);
                return Ok(result);
            }
            else if(doubleMonthDeviceId != 0)
            {
                if (!_sqliteDb.Devices.Any(u => u.Id == doubleMonthDeviceId))
                    return NotFound(new { message = "Device with the ID: " + doubleMonthDeviceId.ToString() + " does not exist." });

                var result = currentPeriodHistoryService.GetUsageHistoryForDeviceFromCurrentMonth(doubleMonthDeviceId);
                return Ok(result);
            }
            else //if (doubleDayDeviceId != 0)
            {
                if (!_sqliteDb.Devices.Any(u => u.Id == doubleDayDeviceId))
                    return NotFound(new { message = "Device with the ID: " + doubleDayDeviceId.ToString() + " does not exist." });

                var result = currentPeriodHistoryService.GetUsageHistoryForDeviceFromCurrentDay(doubleDayDeviceId);
                return Ok(result);
            }
        }
    }
}
