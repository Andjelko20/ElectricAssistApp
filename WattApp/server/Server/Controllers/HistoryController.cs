using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Consumption/Production history for device in past year by month
        /// </summary>
        [HttpGet]
        [Route("YearByMonth/Device/{deviceId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInPastYearByMonth([FromRoute] long deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });

            var HistoryForThePreviousYear = historyService.GetMonthlyEnergyUsage(deviceId);
            return Ok(HistoryForThePreviousYear);
        }

        /// <summary>
        /// Consumption/Production history for device in past week by day
        /// </summary>
        [HttpGet]
        [Route("WeekByDay/Device/{deviceId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInPastWeekByDay([FromRoute] long deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });

            var HistoryForThePreviousMonth = historyService.GetDailyEnergyUsageForPastWeek(deviceId);
            return Ok(HistoryForThePreviousMonth);
        }

        /// <summary>
        /// Consumption/Production for all user`s devices in past year (by month)
        /// </summary>
        [HttpGet]
        [Route("YearByMonth/User/{userId:long}/{deviceCategoryId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetConsumptionByUserForYearByMonth([FromRoute] long userId, [FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

            var HistoryForPastYearByMonthConsumption = historyService.GetMonthlyEnergyUsageForPastYear(userId, deviceCategoryId);
            return Ok(HistoryForPastYearByMonthConsumption);
        }

        /// <summary>
        /// Consumption/Production for all user`s devices for last week (by day)
        /// </summary>
        [HttpGet]
        [Route("WeekByDay/User/{userId:long}/{deviceCategoryId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetUserHistoryForPastWeekByDay([FromRoute] long userId, [FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

            var PredictionForNextWeek = historyService.UserHistoryForThePastWeek(userId, deviceCategoryId);
            return Ok(PredictionForNextWeek);
        }

        /// <summary>
        /// Total device Consumption/Production today
        /// </summary>
        [HttpGet]
        [Route("Today")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceToday([FromQuery] long deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
            {
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });
            }

            var historyList = historyService.GetUsageHistoryForDeviceToday(deviceId);
            return Ok(historyList);
        }

        [HttpGet]
        [Route("Pagination/{pageNumber:int}/{itemsPerPage:int}")]
        public async Task<IActionResult> GetHistoryResultsPagination([FromRoute] int pageNumber, [FromRoute] int itemsPerPage, [FromQuery] long PastMonthByDayDeviceId, long PastMonthByDayUserId, long deviceCategoryId, long PastMonthByDaySettlementId, long PastMonthByDayCityId, long PastDayByHourUserId)
        {
            if(PastMonthByDayDeviceId != 0)
            {
                if (!_sqliteDb.Devices.Any(d => d.Id == PastMonthByDayDeviceId))
                    return NotFound(new {message = "Device with the ID: " + PastMonthByDayDeviceId.ToString() + " does not exist."});

                var result = historyService.GetDailyEnergyUsageForPastMonthPagination(PastMonthByDayDeviceId, pageNumber, itemsPerPage);
                return Ok(result);
            }
            else if(PastMonthByDayUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == PastMonthByDayUserId))
                    return NotFound(new { message = "User with the ID: " + PastMonthByDayUserId.ToString() + " does not exist." });

                var result = historyService.GetProsumerDailyEnergyUsageForPastMonthPagination(PastMonthByDayUserId, deviceCategoryId, pageNumber, itemsPerPage);
                return Ok(result);
            }
            else if (PastMonthByDaySettlementId != 0)
            {
                if (!_sqliteDb.Settlements.Any(s => s.Id == PastMonthByDaySettlementId))
                    return NotFound(new { message = "Settlement with the ID: " + PastMonthByDaySettlementId.ToString() + " does not exist." });

                var result = historyService.GetSettlementDailyEnergyUsageForPastMonthPagination(PastMonthByDaySettlementId, deviceCategoryId, pageNumber, itemsPerPage);
                return Ok(result);
            }
            else if (PastMonthByDayCityId != 0)
            {
                if (!_sqliteDb.Cities.Any(s => s.Id == PastMonthByDayCityId))
                    return NotFound(new { message = "City with the ID: " + PastMonthByDayCityId.ToString() + " does not exist." });

                var result = historyService.GetCityDailyEnergyUsageForPastMonthPagination(PastMonthByDayCityId, deviceCategoryId, pageNumber, itemsPerPage);
                return Ok(result);
            }
            else // if (PastDayByHourUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == PastDayByHourUserId))
                    return NotFound(new { message = "User with the ID: " + PastDayByHourUserId.ToString() + " does not exist." });

                var result = historyService.UserHistoryForThePastDayByHourPagination(PastDayByHourUserId, deviceCategoryId, pageNumber, itemsPerPage);
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("ThatYear/{yearNumber:int}")]
        public async Task<IActionResult> GetHistoryResultsForYear([FromRoute] int yearNumber, [FromQuery] long deviceCategoryId, long cityId, long settlementId, long userId, long deviceId)
        {
            if (cityId != 0)
            {
                if (!_sqliteDb.Cities.Any(c => c.Id == cityId))
                    return NotFound(new { message = "City with the ID: " + cityId.ToString() + " does not exist." });

                var result = historyService.CityHistoryForYearByMonth(cityId, deviceCategoryId, yearNumber);
                return Ok(result);
            }
            else if (settlementId != 0)
            {
                if (!_sqliteDb.Settlements.Any(s => s.Id == settlementId))
                    return NotFound(new { message = "Settlement with the ID: " + settlementId.ToString() + " does not exist." });

                var result = historyService.SettlementHistoryForYearByMonth(settlementId, deviceCategoryId, yearNumber);
                return Ok(result);
            }
            else if (userId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == userId))
                    return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

                var result = historyService.UserHistoryForYearByMonth(userId, deviceCategoryId, yearNumber);
                return Ok(result);
            }
            else
            {
                if (!_sqliteDb.Devices.Any(d => d.Id == deviceId))
                    return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });

                var result = historyService.DeviceHistoryForYearByMonth(deviceId, yearNumber);
                return Ok(result);
            }
        }
    }
}
