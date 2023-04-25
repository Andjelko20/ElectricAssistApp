using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Models.DropDowns.Devices;
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
        /// Device Consumption/Production for current year, month, day
        /// </summary>
        [HttpGet]
        [Route("device")]
        public async Task<IActionResult> GetHistoryForDeviceFromCurrentYear([FromQuery] long doubleYearDeviceId, long doubleMonthDeviceId, long doubleDayDeviceId,
                                                                                        long yearByMonthDeviceId, long monthByDayDeviceId)
        {
            if (doubleYearDeviceId != 0)
            {
                if (!_sqliteDb.Devices.Any(u => u.Id == doubleYearDeviceId))
                    return NotFound(new { message = "Device with the ID: " + doubleYearDeviceId.ToString() + " does not exist." });

                var result = currentPeriodHistoryService.GetUsageHistoryForDeviceFromCurrentYear(doubleYearDeviceId);
                return Ok(result);
            }
            else if (doubleMonthDeviceId != 0)
            {
                if (!_sqliteDb.Devices.Any(u => u.Id == doubleMonthDeviceId))
                    return NotFound(new { message = "Device with the ID: " + doubleMonthDeviceId.ToString() + " does not exist." });

                var result = currentPeriodHistoryService.GetUsageHistoryForDeviceFromCurrentMonth(doubleMonthDeviceId);
                return Ok(result);
            }
            else if (doubleDayDeviceId != 0)
            {
                if (!_sqliteDb.Devices.Any(u => u.Id == doubleDayDeviceId))
                    return NotFound(new { message = "Device with the ID: " + doubleDayDeviceId.ToString() + " does not exist." });

                var result = currentPeriodHistoryService.GetUsageHistoryForDeviceFromCurrentDay(doubleDayDeviceId);
                return Ok(result);
            }
            else if (yearByMonthDeviceId != 0)
            {
                if (!_sqliteDb.Devices.Any(u => u.Id == yearByMonthDeviceId))
                    return NotFound(new { message = "Device with the ID: " + yearByMonthDeviceId.ToString() + " does not exist." });

                var result = currentPeriodHistoryService.GetUsageHistoryForDeviceFromCurrentYearByMonth(yearByMonthDeviceId);
                return Ok(result);
            }
            else //if (monthByDayDeviceId != 0)
            {
                if (!_sqliteDb.Devices.Any(u => u.Id == monthByDayDeviceId))
                    return NotFound(new { message = "Device with the ID: " + monthByDayDeviceId.ToString() + " does not exist." });

                var result = currentPeriodHistoryService.GetUsageHistoryForDeviceFromCurrentMonthByDay(monthByDayDeviceId);
                return Ok(result);
            }
        }

        /// <summary>
        /// Prosumer Consumption/Production for current year, month, day
        /// </summary>
        [HttpGet]
        [Route("user")]
        public async Task<IActionResult> GetHistoryForDeviceFromCurrentYear([FromQuery] long deviceCategoryId, long dayByHourUserId, long monthByDayUserId)
        {
            if(dayByHourUserId!=0)
            { 
                if (!_sqliteDb.Users.Any(u => u.Id == dayByHourUserId))
                    return NotFound(new { message = "User with the ID: " + dayByHourUserId.ToString() + " does not exist." });

                if (!_sqliteDb.Devices.Any(u => u.UserId == dayByHourUserId))
                    return NotFound(new { message = "User with the ID: " + dayByHourUserId.ToString() + " does not have registered devices." }); // nema prijavljen uredjaj, tako da mu je predikcija 0 - ili da vratim neki drugi status?

                if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                var resultsPastDayByHour = currentPeriodHistoryService.GetUsageHistoryForProsumerFromCurrentDayByHour(dayByHourUserId, deviceCategoryId);
                return Ok(resultsPastDayByHour);
            }
            else //if(monthByDayUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == monthByDayUserId))
                    return NotFound(new { message = "User with the ID: " + monthByDayUserId.ToString() + " does not exist." });

                if (!_sqliteDb.Devices.Any(u => u.UserId == monthByDayUserId))
                    return NotFound(new { message = "User with the ID: " + monthByDayUserId.ToString() + " does not have registered devices." }); // nema prijavljen uredjaj, tako da mu je predikcija 0 - ili da vratim neki drugi status?

                if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                var resultsPastDayByHour = currentPeriodHistoryService.GetUsageHistoryForProsumerFromCurrentDayByHour(monthByDayUserId, deviceCategoryId);
                return Ok(resultsPastDayByHour);
            }
        }
    }
}
