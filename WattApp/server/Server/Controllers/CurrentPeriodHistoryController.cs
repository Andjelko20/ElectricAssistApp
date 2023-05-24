using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;
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

        /*/// <summary>
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
        }*/

        /// <summary>
        /// Prosumer Consumption/Production for current year, month, day
        /// </summary>
        [HttpGet]
        [Route("user")]
        public async Task<IActionResult> GetHistoryForProsumerFromCurrentYear([FromQuery] long deviceCategoryId, long dayByHourUserId, long monthByDayUserId, long yearByMonthUserId,
                                                                                          long doubleTodayUserId, long doubleMonthUserId, long doubleYearUserId)
        {
            /*if (dayByHourUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == dayByHourUserId))
                    return NotFound(new { message = "User with the ID: " + dayByHourUserId.ToString() + " does not exist." });

                //if (!_sqliteDb.Devices.Any(u => u.UserId == dayByHourUserId))
                //    return NotFound(new { message = "User with the ID: " + dayByHourUserId.ToString() + " does not have registered devices." }); // nema prijavljen uredjaj, tako da mu je predikcija 0 - ili da vratim neki drugi status?

                if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                var resultsPastDayByHour = currentPeriodHistoryService.GetUsageHistoryForProsumerFromCurrentDayByHour(dayByHourUserId, deviceCategoryId);
                return Ok(resultsPastDayByHour);
            }
            else if (monthByDayUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == monthByDayUserId))
                    return NotFound(new { message = "User with the ID: " + monthByDayUserId.ToString() + " does not exist." });

                //if (!_sqliteDb.Devices.Any(u => u.UserId == monthByDayUserId))
                //   return NotFound(new { message = "User with the ID: " + monthByDayUserId.ToString() + " does not have registered devices." }); // nema prijavljen uredjaj, tako da mu je predikcija 0 - ili da vratim neki drugi status?

                if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                var results = currentPeriodHistoryService.GetUsageHistoryForProsumerFromCurrentMonthByDay(monthByDayUserId, deviceCategoryId);
                return Ok(results);
            }
            else if (yearByMonthUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == yearByMonthUserId))
                    return NotFound(new { message = "User with the ID: " + yearByMonthUserId.ToString() + " does not exist." });

                //if (!_sqliteDb.Devices.Any(u => u.UserId == yearByMonthUserId))
                //    return NotFound(new { message = "User with the ID: " + yearByMonthUserId.ToString() + " does not have registered devices." }); // nema prijavljen uredjaj, tako da mu je predikcija 0 - ili da vratim neki drugi status?

                if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                var results = currentPeriodHistoryService.GetUsageHistoryForProsumerFromCurrentYearByMonth(yearByMonthUserId, deviceCategoryId);
                return Ok(results);
            }
            else */if (doubleTodayUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == doubleTodayUserId))
                    return NotFound(new { message = "User with the ID: " + doubleTodayUserId.ToString() + " does not exist." });

                //if (!_sqliteDb.Devices.Any(u => u.UserId == doubleTodayUserId))
                //    return NotFound(new { message = "User with the ID: " + doubleTodayUserId.ToString() + " does not have registered devices." }); // nema prijavljen uredjaj, tako da mu je predikcija 0 - ili da vratim neki drugi status?

                if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                var results = currentPeriodHistoryService.GetUsageHistoryForProsumerFromCurrentDay(doubleTodayUserId, deviceCategoryId);
                return Ok(results);
            }
            else if (doubleMonthUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == doubleMonthUserId))
                    return NotFound(new { message = "User with the ID: " + doubleMonthUserId.ToString() + " does not exist." });

                //if (!_sqliteDb.Devices.Any(u => u.UserId == doubleMonthUserId))
                //    return NotFound(new { message = "User with the ID: " + doubleMonthUserId.ToString() + " does not have registered devices." }); // nema prijavljen uredjaj, tako da mu je predikcija 0 - ili da vratim neki drugi status?

                if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                var results = currentPeriodHistoryService.GetUsageHistoryForProsumerFromCurrentMonth(doubleMonthUserId, deviceCategoryId);
                return Ok(results);
            }
            else //if (doubleYearUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == doubleYearUserId))
                    return NotFound(new { message = "User with the ID: " + doubleYearUserId.ToString() + " does not exist." });

                //if (!_sqliteDb.Devices.Any(u => u.UserId == doubleYearUserId))
                //    return NotFound(new { message = "User with the ID: " + doubleYearUserId.ToString() + " does not have registered devices." }); // nema prijavljen uredjaj, tako da mu je predikcija 0 - ili da vratim neki drugi status?

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
