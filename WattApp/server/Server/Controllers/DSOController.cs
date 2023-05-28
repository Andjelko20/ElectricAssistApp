﻿using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Services;

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
        /// 1.) Get (settlementId, settlementName) --- 2.) Consumption/Production for settlement - today by hour
        /// </summary>
        [HttpGet]
        [Route("Settlement/")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetSettlements([FromQuery] long cityId)
        {
            if (!_sqliteDb.Cities.Any(c => c.Id == cityId))
                return NotFound(new { message = "City with ID: " + cityId.ToString() + " does not exist." });

            var settlements = dsoService.GetSettlements(cityId);

            if (settlements == null)
                return NotFound(new { message = "Settlements for city with ID: " + cityId.ToString() + " don`t exist." });

            return Ok(settlements);
        }

        /// <summary>
        /// 1.) Get double consumptio/production for cityId - today or this month || 2.) Get cityId
        /// </summary>
        [HttpGet]
        [Route("City/")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetCity([FromQuery] long deviceCategoryId, [FromQuery] long todayByHourCityId, long todayCityId, long thisMonthCityId, long thisYearCityId, string cityName="null")
        {
            if(!cityName.Equals("null"))
            {
                var cityId = dsoService.GetCityId(cityName);

                if (cityId == -1)
                    return NotFound(new { message = "City with name: " + cityName.ToString() + " does not exist." });

                return Ok(cityId);
            }

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
            else if (todayByHourCityId != 0)
            {
                if (!_sqliteDb.Cities.Any(c => c.Id == todayByHourCityId))
                    return NotFound(new { message = "City with ID: " + todayByHourCityId.ToString() + " doesnt exist" });

                return Ok(dsoService.CalculateEnergyUsageForTodayInCity(todayByHourCityId, deviceCategoryId));
            }

            return BadRequest("Input parameters are empty.");
        }

        /// <summary>
        /// Pagination for table
        /// </summary>
        [HttpGet]
        [Route("Pagination/")]
        public async Task<IActionResult> GetCityPagination([FromQuery] int pageNumber, int itemsPerPage, long deviceCategoryId, long todayByHourCityId, long todayByHourSettlementId)
        {
            if (pageNumber > 0 && itemsPerPage > 0)
            {
                if (todayByHourCityId != 0)
                {
                    if (!_sqliteDb.Cities.Any(c => c.Id == todayByHourCityId))
                        return NotFound(new { message = "City with ID: " + todayByHourCityId.ToString() + " doesnt exist" });

                    if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                        return NotFound(new { message = "Device category with ID: " + deviceCategoryId.ToString() + " doesnt exist" });

                    return Ok(dsoService.GetCityHistoryTodayByHourPagination(todayByHourCityId, deviceCategoryId, pageNumber, itemsPerPage));
                }
                else
                {
                    if (!_sqliteDb.Settlements.Any(s => s.Id == todayByHourSettlementId))
                        return NotFound(new { message = "Settlement with ID: " + todayByHourSettlementId.ToString() + " doesnt exist" });

                    if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                        return NotFound(new { message = "Device category with ID: " + deviceCategoryId.ToString() + " doesnt exist" });

                    return Ok(dsoService.GetSettlementHistoryTodayByHourPagination(todayByHourSettlementId, deviceCategoryId, pageNumber, itemsPerPage));
                }
            }
            else
                return NotFound(new { message = "Value for pageNumber and itemsPerPage must be greater then 0." });
        }
    }
}
