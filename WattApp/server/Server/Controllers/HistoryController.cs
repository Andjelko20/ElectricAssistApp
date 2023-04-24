using Microsoft.AspNetCore.Authorization;
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
        /// Total device Consumption/Production in last year
        /// </summary>
        [HttpGet]
        [Route("Year/Device/{deviceId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInLastYear([FromRoute] long deviceId)
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
        /// Total device Consumption/Production in last month
        /// </summary>
        [HttpGet]
        [Route("Month/Device/{deviceId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInLastMonth([FromRoute] long deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
            {
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });
            }

            var historyList = historyService.GetUsageHistoryForDeviceInLastMonth(deviceId);
            return Ok(historyList);
        }

        /// <summary>
        /// Total device Consumption/Production in last day
        /// </summary>
        [HttpGet]
        [Route("Day/Device/{deviceId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInLastDay([FromRoute] long deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceEnergyUsages.Any(u => u.DeviceId == deviceId))
                return Ok(0.0); // jer je potrosnja 0 ako nije paljen

            var HistoryForThePreviousDay = historyService.GetUsageHistoryForDeviceInLastDay(deviceId);
            return Ok(HistoryForThePreviousDay);
        }

        /// <summary>
        /// Total device Consumption/Production in last week
        /// </summary>
        [HttpGet]
        [Route("Week/Device/{deviceId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInPastWeek([FromRoute] long deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceEnergyUsages.Any(u => u.DeviceId == deviceId))
                return Ok(0.0); // jer je potrosnja 0 ako nije paljen

            var HistoryForThePreviousDay = historyService.GetUsageHistoryForDeviceInPastWeek(deviceId);
            return Ok(HistoryForThePreviousDay);
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
        /// Consumption/Production history for device in past month by day
        /// </summary>
        [HttpGet]
        [Route("MonthByDay/Device/{deviceId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInPastMonthByDay([FromRoute] long deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });

            var HistoryForThePreviousMonth = historyService.GetDailyEnergyUsageForPastMonth(deviceId);
            return Ok(HistoryForThePreviousMonth);
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
        /// Consumption/Production for all user`s devices
        /// </summary>
        [HttpGet]
        [Route("TotalConsumption/User/{userId:long}/{deviceCategoryId}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetTotalConsumptionByUser([FromRoute] long userId, [FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            if (!_sqliteDb.Devices.Any(u => u.UserId == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not have registered devices." });

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });
            /*
            if (!_sqliteDb.Devices.Any(u => u.DeviceCategoryId == deviceCategoryId))
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });
			*/

            if (!_sqliteDb.Devices.Include(d => d.DeviceModel).ThenInclude(dm => dm.DeviceType).ThenInclude(dt => dt.DeviceCategory).Any(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId))
            {
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });
            }

            var HistoryForTotalConsumption = historyService.GetTotalEnergyConsumptionForUser(userId, deviceCategoryId);
            return Ok(HistoryForTotalConsumption);
        }

        /// <summary>
        /// Consumption/Production for user, past day
        /// </summary>
        [HttpGet]
        [Route("Day/User/{userId:long}/{deviceCategoryId}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryByUserForPastDay([FromRoute] long userId, [FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            if (!_sqliteDb.Devices.Any(u => u.UserId == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not have registered devices." });

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });
            /*
            if (!_sqliteDb.Devices.Any(u => u.DeviceCategoryId == deviceCategoryId))
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });
			*/

            if (!_sqliteDb.Devices.Include(d => d.DeviceModel).ThenInclude(dm => dm.DeviceType).ThenInclude(dt => dt.DeviceCategory).Any(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId))
            {
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });
            }

            var HistoryForPastDay = historyService.GetUserEnergyConsumptionForPastDay(userId, deviceCategoryId);
            return Ok(HistoryForPastDay);
        }

        /// <summary>
        /// Consumption/Production for user in past week
        /// </summary>
        [HttpGet]
        [Route("Week/User/{userId:long}/{deviceCategoryId}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryByUserForPastWeek([FromRoute] long userId, [FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            if (!_sqliteDb.Devices.Any(u => u.UserId == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not have registered devices." });

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });
            /*
            if (!_sqliteDb.Devices.Any(u => u.DeviceCategoryId == deviceCategoryId))
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });
			*/

            if (!_sqliteDb.Devices.Include(d => d.DeviceModel).ThenInclude(dm => dm.DeviceType).ThenInclude(dt => dt.DeviceCategory).Any(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId))
            {
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });
            }

            var HistoryForPastWeek = historyService.GetUserEnergyConsumptionForPastWeek(userId, deviceCategoryId);
            return Ok(HistoryForPastWeek);
        }

        /// <summary>
        /// Consumption/Production for user in past month
        /// </summary>
        [HttpGet]
        [Route("Month/User/{userId:long}/{deviceCategoryId}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryByUserForPastMonth([FromRoute] long userId, [FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            if (!_sqliteDb.Devices.Any(u => u.UserId == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not have registered devices." });

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });
            /*
            if (!_sqliteDb.Devices.Any(u => u.DeviceCategoryId == deviceCategoryId))
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });
			*/

            if (!_sqliteDb.Devices.Include(d => d.DeviceModel).ThenInclude(dm => dm.DeviceType).ThenInclude(dt => dt.DeviceCategory).Any(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId))
            {
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });
            }

            var HistoryForPastMonth = historyService.GetUserEnergyConsumptionForPastMonth(userId, deviceCategoryId);
            return Ok(HistoryForPastMonth);
        }

        /// <summary>
        /// Consumption/Production for user in past year
        /// </summary>
        [HttpGet]
        [Route("Year/User/{userId:long}/{deviceCategoryId}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetConsumptionByUserForYear([FromRoute] long userId, [FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            if (!_sqliteDb.Devices.Any(u => u.UserId == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not have registered devices." });

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });
            /*
            if (!_sqliteDb.Devices.Any(u => u.DeviceCategoryId == deviceCategoryId))
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });
			*/

            if (!_sqliteDb.Devices.Include(d => d.DeviceModel).ThenInclude(dm => dm.DeviceType).ThenInclude(dt => dt.DeviceCategory).Any(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId))
            {
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });
            }

            var HistoryForPastYearConsumption = historyService.GetUserEnergyConsumptionForPastYear(userId, deviceCategoryId);
            return Ok(HistoryForPastYearConsumption);
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

            if (!_sqliteDb.Devices.Any(u => u.UserId == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not have registered devices." });

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });
            /*
            if (!_sqliteDb.Devices.Any(u => u.DeviceCategoryId == deviceCategoryId))
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });
			*/

            if (!_sqliteDb.Devices.Include(d => d.DeviceModel).ThenInclude(dm => dm.DeviceType).ThenInclude(dt => dt.DeviceCategory).Any(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId))
            {
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });
            }

            var HistoryForPastYearByMonthConsumption = historyService.GetMonthlyEnergyUsageForPastYear(userId, deviceCategoryId);
            return Ok(HistoryForPastYearByMonthConsumption);
        }

        /// <summary>
        /// Consumption/Production for all user`s devices for last month (by day)
        /// </summary>
        [HttpGet]
        [Route("MonthByDay/User/{userId:long}/{deviceCategoryId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetUserHistoryForPastMonthByDay([FromRoute] long userId, [FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            if (!_sqliteDb.Devices.Any(u => u.UserId == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not have registered devices." }); // nema prijavljen uredjaj, tako da mu je predikcija 0 - ili da vratim neki drugi status?

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

            if (!_sqliteDb.Devices.Include(d => d.DeviceModel).ThenInclude(dm => dm.DeviceType).ThenInclude(dt => dt.DeviceCategory).Any(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId))
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });

            var historyResult = historyService.UserHistoryForThePastMonth(userId, deviceCategoryId);
            return Ok(historyResult);
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

            if (!_sqliteDb.Devices.Any(u => u.UserId == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not have registered devices." }); // nema prijavljen uredjaj, tako da mu je predikcija 0 - ili da vratim neki drugi status?

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });
            /*
            if (!_sqliteDb.Devices.Any(u => u.DeviceCategoryId == deviceCategoryId))
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });
			*/

            if (!_sqliteDb.Devices.Include(d => d.DeviceModel).ThenInclude(dm => dm.DeviceType).ThenInclude(dt => dt.DeviceCategory).Any(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId))
            {
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });
            }

            var PredictionForNextWeek = historyService.UserHistoryForThePastWeek(userId, deviceCategoryId);
            return Ok(PredictionForNextWeek);
        }

        /// <summary>
        /// Consumption/Production for all user`s devices for last week (by day)
        /// </summary>
        [HttpGet]
        [Route("DayByHour/User/{userId:long}/{deviceCategoryId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetUserHistoryForPastDayByHour([FromRoute] long userId, [FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            if (!_sqliteDb.Devices.Any(u => u.UserId == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not have registered devices." }); // nema prijavljen uredjaj, tako da mu je predikcija 0 - ili da vratim neki drugi status?

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

            if (!_sqliteDb.Devices.Include(d => d.DeviceModel).ThenInclude(dm => dm.DeviceType).ThenInclude(dt => dt.DeviceCategory).Any(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId))
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });

            var resultsPastDayByHour = historyService.UserHistoryForThePastDayByHour(userId, deviceCategoryId);
            return Ok(resultsPastDayByHour);
        }

        /// <summary>
        /// Consumption/Production for all users from settlement for last week (by day)
        /// </summary>
        [HttpGet]
        [Route("WeekByDay/Settlement/{settlementId:long}/{deviceCategoryId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetSettlementHistoryForPastWeekByDay([FromRoute] long settlementId, [FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.Settlements.Any(s => s.Id == settlementId))
                return NotFound(new { message = "Settlement with the ID: " + settlementId.ToString() + " does not exist." });

            if (!_sqliteDb.Users.Any(u => u.SettlementId == settlementId))
                return NotFound(new { message = "Settlement with the ID: " + settlementId.ToString() + " does not have registered users." }); // nema prijavljen uredjaj, tako da mu je predikcija 0 - ili da vratim neki drugi status?

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });


            return Ok(historyService.SettlementHistoryForThePastWeek(settlementId, deviceCategoryId));
        }

        /// <summary>
        /// Consumption/Production for all users from city for last week (by day)
        /// </summary>
        [HttpGet]
        [Route("WeekByDay/City/{cityId:long}/{deviceCategoryId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetCityHistoryForPastWeekByDay([FromRoute] long cityId, [FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.Cities.Any(c => c.Id == cityId))
                return NotFound(new { message = "City with the ID: " + cityId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

            return Ok(historyService.CityHistoryForThePastWeek(cityId, deviceCategoryId));
        }

        /// <summary>
        /// Consumption/Production for all users from settlement for last month (by day)
        /// </summary>
        [HttpGet]
        [Route("MonthByDay/Settlement/{settlementId:long}/{deviceCategoryId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetSettlementHistoryForPastMonthByDay([FromRoute] long settlementId, [FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.Settlements.Any(s => s.Id == settlementId))
                return NotFound(new { message = "City with the ID: " + settlementId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

            return Ok(historyService.SettlementHistoryForThePastMonth(settlementId, deviceCategoryId));
        }

        /// <summary>
        /// Consumption/Production for all users from city for last month (by day)
        /// </summary>
        [HttpGet]
        [Route("MonthByDay/City/{cityId:long}/{deviceCategoryId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetCityHistoryForPastMonthByDay([FromRoute] long cityId, [FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.Cities.Any(c => c.Id == cityId))
                return NotFound(new { message = "City with the ID: " + cityId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

            return Ok(historyService.CityHistoryForThePastMonth(cityId, deviceCategoryId));
        }

        /// <summary>
        /// Consumption/Production for all users from city for last year (by month)
        /// </summary>
        [HttpGet]
        [Route("YearByMonth/City/{cityId:long}/{deviceCategoryId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetCityHistoryForPastYearByMonth([FromRoute] long cityId, [FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.Cities.Any(c => c.Id == cityId))
                return NotFound(new { message = "City with the ID: " + cityId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

            return Ok(historyService.CityHistoryForThePastYearByMonth(cityId, deviceCategoryId));
        }

        /// <summary>
        /// Consumption/Production for all users from settlement for last year (by month)
        /// </summary>
        [HttpGet]
        [Route("YearByMonth/Settlement/{settlementId:long}/{deviceCategoryId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetSettlementHistoryForPastYearByMonth([FromRoute] long settlementId, [FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.Settlements.Any(s => s.Id == settlementId))
                return NotFound(new { message = "Settlement with the ID: " + settlementId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

            return Ok(historyService.SettlementHistoryForThePastYearByMonth(settlementId, deviceCategoryId));
        }

        /// <summary>
        /// Total device Consumption/Production in this month
        /// </summary>
        [HttpGet]
        [Route("Month")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInThisMonth([FromQuery] long deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
            {
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });
            }

            var historyList = historyService.GetUsageHistoryForDeviceInThisMonth(deviceId);
            return Ok(historyList);
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

        /// <summary>
        /// Total device Consumption/Production this year
        /// </summary>
        [HttpGet]
        [Route("Year")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceThisYear([FromQuery] long deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
            {
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });
            }

            var historyList = historyService.GetUsageHistoryForDeviceThisYear(deviceId);
            return Ok(historyList);
        }

        /// <summary>
        /// Total device Consumption/Production this year
        /// </summary>
        [HttpGet]
        [Route("PreviousMonth")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDevicePreviousMonth([FromQuery] long deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
            {
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });
            }

            var historyList = historyService.GetUsageHistoryForDeviceForPreviousMonth(deviceId);
            return Ok(historyList);
        }
    }
}
