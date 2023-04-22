using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Server.Data;
using Server.Models;
using Server.Services;
using Server.Models.DropDowns.Location;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DSOController : Controller
    {
        public readonly SqliteDbContext _sqliteDb;
        public readonly IDSOService dsoService;

        public DSOController(SqliteDbContext sqliteDb, IDSOService dsoService)
        {
            _sqliteDb = sqliteDb;
            this.dsoService = dsoService;
        }

        /// <summary>
        /// Get CityId
        /// </summary>
        [HttpGet]
        [Route("City/")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetCity([FromQuery] string cityName)
        {
            var cityId = dsoService.GetCityId(cityName);

            if (cityId == -1)
                return NotFound(new { message = "City with name: " + cityName.ToString() + " does not exist." });

            return Ok(cityId);
        }

        /// <summary>
        /// Get (settlementId, settlementName)
        /// </summary>
        [HttpGet]
        [Route("Settlement/")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetSettlements([FromQuery] long cityId)
        {
            if (!_sqliteDb.Cities.Any(c => c.Id == cityId))
                return NotFound(new { message = "City with ID: " + cityId.ToString() + " does not exist." });

            var settlements = dsoService.GetSettlements(cityId);
            
            if(settlements == null)
                return NotFound(new { message = "Settlements for city with ID: " + cityId.ToString() + " don`t exist." });

            return Ok(settlements);
        }

        /// <summary>
        /// Get double consumptio/production for cityId - today or this month
        /// </summary>
        [HttpGet]
        [Route("CityDouble/{deviceCategoryId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetCity([FromRoute] long deviceCategoryId, [FromQuery] long todayCityId, long thisMonthCityId, long thisYearCityId)
        {
            if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with ID: " + deviceCategoryId.ToString() + " doesnt exist"});        

            if (todayCityId!=0)
            {
                if (!_sqliteDb.Cities.Any(c => c.Id == todayCityId))
                    return NotFound(new { message = "City with ID: " + todayCityId.ToString() + " doesnt exist" });
                
                return Ok(dsoService.GetCityConsumptionForToday(todayCityId, deviceCategoryId));
            } 
            else if(thisMonthCityId!=0)
            {
                if (!_sqliteDb.Cities.Any(c => c.Id == thisMonthCityId))
                    return NotFound(new { message = "City with ID: " + thisMonthCityId.ToString() + " doesnt exist" });

                return Ok(dsoService.GetUsageHistoryForDeviceInThisMonth(thisMonthCityId, deviceCategoryId));
            }
            else if(thisYearCityId!=0)
            {
                if (!_sqliteDb.Cities.Any(c => c.Id == thisYearCityId))
                    return NotFound(new { message = "City with ID: " + thisYearCityId.ToString() + " doesnt exist" });

                return Ok(dsoService.GetUsageHistoryForDeviceInThisYear(thisYearCityId, deviceCategoryId));
            }

            return BadRequest("Input parameters are empty.");
        }

        /// <summary>
        /// Consumption/Production for settlement - today by hour
        /// </summary>
        [HttpGet]
        [Route("SettlementDayByHour/")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetSettlementDayByHour([FromQuery] long settlementId, [FromQuery] long deviceCategoryId)
        {
            if (!_sqliteDb.Settlements.Any(s => s.Id == settlementId))
                return NotFound(new { message = "Settlement with ID: " + settlementId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with ID: " + deviceCategoryId.ToString() + " does not exist." });

            var resultList = dsoService.CalculateEnergyUsageForToday(settlementId, deviceCategoryId);

            return Ok(resultList);
        }
    }
}
