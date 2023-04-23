using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Models;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;
using Server.Services;
using System.Data;
using System.Linq;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProsumerController : Controller
    {
        public readonly SqliteDbContext _sqliteDb;
        public readonly IProsumerService prosumerService;

        public ProsumerController(SqliteDbContext sqliteDb, IProsumerService prosumerService)
        {
            _sqliteDb = sqliteDb;
            this.prosumerService = prosumerService;
        }

        /// <summary>
        /// Total Consumption/Production in The Moment (country)
        /// </summary>
        [HttpGet]
        //[Route("{deviceCategoryName:regex(Electricity Producer|Electricity Consumer|Electricity Stock)}/{cityOrSettlement:regex(city|settlement)}/{settlementName}")]
        [Route("{deviceCategoryId:long}")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetTotalConsumptionInTheMomentForCategory([FromRoute] long deviceCategoryId)
        {
            /*if(!_sqliteDb.Settlements.Any(s => EF.Functions.Like(s.Name, $"%{settlementName}%")))
            {
                return NotFound(new { message = "Settlement with the name: " + settlementName.ToString() + " does not exist." });
            }*/

            if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                return NotFound(new { message = "Device Category with the ID: " + deviceCategoryId.ToString() + " does not exist." });
            return Ok(prosumerService.GetTotalConsumptionInTheMoment(deviceCategoryId));
        }

        /// <summary>
        /// Total Consumption/Production in The Moment (Settlement)
        /// </summary>
        [HttpGet]
        [Route("{deviceCategoryId:long}/{settlementId:long}")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetTotalConsumptionInTheMomentForCategoryInSettlement([FromRoute] long deviceCategoryId, [FromRoute] long settlementId)
        {
            if(!_sqliteDb.Settlements.Any(s => s.Id == settlementId))
                return NotFound(new { message = "Settlement with the ID: " + settlementId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID: " + deviceCategoryId.ToString() + " does not exist." });

            return Ok(prosumerService.GetTotalConsumptionInTheMomentForSettlement(deviceCategoryId, settlementId));
        }

        /// <summary>
        /// Total Consumption/Production in The Moment (City)
        /// </summary>
        [HttpGet]
        [Route("city/{deviceCategoryId:long}/{cityId:long}")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetTotalConsumptionInTheMomentForCategoryInCity([FromRoute] long deviceCategoryId, [FromRoute] long cityId)
        {
            if (!_sqliteDb.Cities.Any(c => c.Id == cityId))
            {
                return NotFound(new { message = "City with the ID: " + cityId.ToString() + " does not exist." });
            }

            if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID: " + deviceCategoryId.ToString() + " does not exist." });
                
            return Ok(prosumerService.GetTotalConsumptionInTheMomentForCity(deviceCategoryId, cityId));
        }

        /// <summary>
        /// Total Consumption/Production in The Moment (one prosumer)
        /// </summary>
        [HttpGet]
        [Route("user/{deviceCategoryId:long}/{userId:long}")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetTotalConsumptionInTheMomentForCategoryOneProsumer([FromRoute] long deviceCategoryId, [FromRoute] long userId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
            {
                return NotFound(new { message = "User with the id: " + userId.ToString() + " does not exist." });
            }

            if (!_sqliteDb.Devices.Any(d => d.UserId == userId))
            {
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not have registered devices." });
            }

            if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID: " + deviceCategoryId.ToString() + " does not exist." });

            return Ok(prosumerService.GetTotalConsumptionInTheMomentForOneProsumer(deviceCategoryId, userId));
        }
        
        /// <summary>
        /// Average Consumption/Production in The Moment (settlement)
        /// </summary>
        [HttpGet]
        [Route("avg/{deviceCategoryId:long}/{settlementId:long}")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetAvgConsumptionInTheMomentForSettlement([FromRoute] long deviceCategoryId, [FromRoute] long settlementId)
        {
            if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID: " + deviceCategoryId.ToString() + " does not exist." });

            if (!_sqliteDb.Settlements.Any(s => s.Id == settlementId))
            {
                return NotFound(new { message = "Settlement with the ID: " + settlementId.ToString() + " does not exist." });
            }

            var energy = prosumerService.GetTotalConsumptionInTheMomentForSettlement(deviceCategoryId, settlementId);
            var averageEnergy = prosumerService.GetAverageConsumptionInTheMomentForSettlement(settlementId, energy);
            return Ok(averageEnergy);
        }

        /// <summary>
        /// Average Consumption/Production in The Moment (city)
        /// </summary>
        [HttpGet]
        [Route("average/{deviceCategoryId:long}/{cityId:long}")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetAvgConsumptionInTheMomentForCity([FromRoute] long deviceCategoryId, [FromRoute] long cityId)
        {
            if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID: " + deviceCategoryId.ToString() + " does not exist." });
            
            if (!_sqliteDb.Cities.Any(c => c.Id == cityId))
                return NotFound(new { message = "Device category with the ID: " + deviceCategoryId.ToString() + " does not exist." });

            var energy = prosumerService.GetTotalConsumptionInTheMomentForCity(deviceCategoryId, cityId);
            var averageEnergy = prosumerService.GetAverageConsumptionInTheMomentForCity(cityId, energy);
            return Ok(averageEnergy);
        }
        
        /// <summary>
        /// Average Consumption/Production in The Moment (country)
        /// </summary>
        [HttpGet]
        [Route("average/{deviceCategoryId:long}")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetAvgConsumptionInTheMomentForCountry([FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                return NotFound(new { message = "Device Category with the ID: " + deviceCategoryId.ToString() + " does not exist." });

            var energy = prosumerService.GetTotalConsumptionInTheMoment(deviceCategoryId);
            var averageEnergy = prosumerService.GetAverageConsumptionProductionInTheMomentForAllProsumers(energy);
            return Ok(averageEnergy);
        }

        /// <summary>
        /// Total number of consumption/production devices from all prosumers in the city or settlement
        /// </summary>
        [HttpGet]
        [Route("{CityOrSettlement:regex(city|settlement)}/{CityId:long}/{SettlementId:long}/{DeviceCategoryId:long}")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetTotalNumberOfDevicesInTheCityOrSettlement([FromRoute] string CityOrSettlement, [FromRoute] long CityId, [FromRoute] long SettlementId, [FromRoute] long DeviceCategoryId)
        {
            double TotalResult = -1;

            if (CityOrSettlement.ToLower() == "city")
            {
                TotalResult = prosumerService.GetTotalNumberOfDevicesInTheCity(DeviceCategoryId, CityId);
                return Ok(TotalResult);
            }
            else if(CityOrSettlement.ToLower() == "settlement")
            {
                TotalResult = prosumerService.GetTotalNumberOfDevicesInTheSettlement(DeviceCategoryId, CityId, SettlementId);
                return Ok(TotalResult);
            }
            return BadRequest("Invalid parameter. Please use 'city' or 'settlement'.");
        }

        /// <summary>
        /// 1.) Today`s Consumption/Production for device (by hour) || 2.) Today`s Consumption/Production for one prosumer (double)
        /// </summary>
        [HttpGet]
        [Route("today")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetDeviceEnergyFromTodaysDay([FromQuery] long deviceId, long doubleTodayUserId, long deviceCategoryId)
        {
            if(deviceId != 0)
            {
                List<EnergyToday> energyTodayList = prosumerService.CalculateEnergyUsageForToday(deviceId);
                if (energyTodayList == null)
                    return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });

                return Ok(energyTodayList);
            }
            else //if(doubleTodayUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == doubleTodayUserId))
                    return NotFound(new { message = "User with the ID: " + doubleTodayUserId.ToString() + " does not exist." });

                if (!_sqliteDb.Devices.Any(u => u.UserId == doubleTodayUserId))
                    return NotFound(new { message = "User with the ID: " + doubleTodayUserId.ToString() + " does not have registered devices." });

                if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });


                double energyUsageToday = prosumerService.GetUserEnergyConsumptionForToday(doubleTodayUserId, deviceCategoryId);
                return Ok(energyUsageToday);
            }
        }

        /// <summary>
        /// 1.) This month Consumption/Production for one prosumer (double)
        /// </summary>
        [HttpGet]
        [Route("month")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetProsumerEnergyFromThisMonth(long doubleMonthUserId, long deviceCategoryId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == doubleMonthUserId))
                return NotFound(new { message = "User with the ID: " + doubleMonthUserId.ToString() + " does not exist." });

            if (!_sqliteDb.Devices.Any(u => u.UserId == doubleMonthUserId))
                return NotFound(new { message = "User with the ID: " + doubleMonthUserId.ToString() + " does not have registered devices." });

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });


            double energyUsageMonth = prosumerService.GetUserEnergyConsumptionForThisMonth(doubleMonthUserId, deviceCategoryId);
            return Ok(energyUsageMonth);
        }

        [HttpGet]
        [Route("year")]
        public async Task<IActionResult> GetProsumerEnergyForThisYear(long doubleYearUserId, long deviceCategoryId)
        {
            double energyUsageYear = prosumerService.GetUserEnergyConsumptionForThisYear(doubleYearUserId, deviceCategoryId);
            return Ok(energyUsageYear);
        }

        /// <summary>
        /// Number of devices from one prosumer
        /// </summary>
        [HttpGet]
        [Route("numberOfDevices/{userId:long}")]
        public async Task<IActionResult> GetNumberOfDevicesOfOneProsumer([FromRoute] long userId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            return Ok(prosumerService.GetNumberOfDevicesOfOneProsumer(userId));
        }
    }
}
