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
        /// Prosumer Consumption/Production for current year, month, day
        /// </summary>
        [HttpGet]
        [Route("user")]
        public async Task<IActionResult> GetHistoryForProsumerFromCurrentYear([FromQuery] long deviceCategoryId, long doubleTodayUserId, long doubleMonthUserId, long doubleYearUserId)
        {
            if (doubleTodayUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == doubleTodayUserId))
                    return NotFound(new { message = "User with the ID: " + doubleTodayUserId.ToString() + " does not exist." });

                if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                var results = currentPeriodHistoryService.GetUsageHistoryForProsumerFromCurrentDay(doubleTodayUserId, deviceCategoryId);
                return Ok(results);
            }
            else if (doubleMonthUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == doubleMonthUserId))
                    return NotFound(new { message = "User with the ID: " + doubleMonthUserId.ToString() + " does not exist." });

                if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                var results = currentPeriodHistoryService.GetUsageHistoryForProsumerFromCurrentMonth(doubleMonthUserId, deviceCategoryId);
                return Ok(results);
            }
            else //if (doubleYearUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == doubleYearUserId))
                    return NotFound(new { message = "User with the ID: " + doubleYearUserId.ToString() + " does not exist." });

                if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                var results = currentPeriodHistoryService.GetUsageHistoryForProsumerFromCurrentYear(doubleYearUserId, deviceCategoryId);
                return Ok(results);
            }
        }

        /// <summary>
        /// Pagination
        /// </summary>
        [HttpGet]
        [Route("Pagination")]
        public async Task<IActionResult> GetCurrentHistoryPeriodPagination([FromQuery] int pageNumber, int itemsPerPage, long deviceCategoryId, long dayByHourUserId, long monthByDayUserId, long monthByDayDeviceId)
        {
            if (dayByHourUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == dayByHourUserId))
                    return NotFound(new { message = "User with the ID: " + dayByHourUserId.ToString() + " does not exist." });

                if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID: " + deviceCategoryId.ToString() + " does not exist." });

                var result = currentPeriodHistoryService.GetProsumerTodayByHourEnergyUsagePagination(dayByHourUserId, deviceCategoryId, pageNumber, itemsPerPage);
                return Ok(result);
            }
            else if(monthByDayUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == monthByDayUserId))
                    return NotFound(new { message = "User with the ID: " + monthByDayUserId.ToString() + " does not exist." });

                if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID: " + deviceCategoryId.ToString() + " does not exist." });

                var result = currentPeriodHistoryService.GetProsumerMonthByDayEnergyUsagePagination(monthByDayUserId, deviceCategoryId, pageNumber, itemsPerPage);
                return Ok(result);
            }
            else //if (monthByDayDeviceId != 0)
            {
                if (!_sqliteDb.Devices.Any(d => d.Id == monthByDayDeviceId))
                    return NotFound(new { message = "Device with the ID: " + monthByDayDeviceId.ToString() + " does not exist." });

                var result = currentPeriodHistoryService.GetDeviceMonthByDayEnergyUsagePagination(monthByDayDeviceId, pageNumber, itemsPerPage);
                return Ok(result);
            }
        }
    }
}
