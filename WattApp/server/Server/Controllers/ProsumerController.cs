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

            if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID: " + deviceCategoryId.ToString() + " does not exist." });

            return Ok(prosumerService.GetTotalConsumptionInTheMomentForOneProsumer(deviceCategoryId, userId));
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
                return NotFound(new { message = "City with the ID: " + cityId.ToString() + " does not exist." });

            var energy = prosumerService.GetTotalConsumptionInTheMomentForCity(deviceCategoryId, cityId);
            var averageEnergy = prosumerService.GetAverageConsumptionInTheMomentForCity(cityId, energy);
            return Ok(averageEnergy);
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

        /// <summary>
        /// How much device worked
        /// </summary>
        [HttpPut]
        [Route("device")]
        public async Task<IActionResult> HowMuchDeviceWorked([FromQuery] long deviceId, DateTime turnedOn, DateTime turnedOff)
        {
            if (!_sqliteDb.Devices.Any(d => d.Id == deviceId))
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });

            var response = prosumerService.FromWhenToWhenDeviceWorks(deviceId, turnedOn, turnedOff);
            if (response == null)
                return BadRequest("Failed entry data into the database.");
                    
            return Ok(response);
        }
    }
}
