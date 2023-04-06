using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
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
        /// Total Consumption/Production in The Moment
        /// </summary>
        [HttpGet]
        //[Route("{deviceCategoryName:regex(Electricity Producer|Electricity Consumer|Electricity Stock)}/{cityOrSettlement:regex(city|settlement)}/{settlementName}")]
        [Route("{deviceCategoryName:regex(Electricity Producer|Electricity Consumer|Electricity Stock)}")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetTotalConsumptionInTheMomentForCategory([FromRoute] string deviceCategoryName)
        {
            /*if(!_sqliteDb.Settlements.Any(s => EF.Functions.Like(s.Name, $"%{settlementName}%")))
            {
                return NotFound(new { message = "Settlement with the name: " + settlementName.ToString() + " does not exist." });
            }*/

            if (deviceCategoryName.ToLower().Equals("electricity producer") || deviceCategoryName.ToLower().Equals("electricity consumer") | deviceCategoryName.ToLower().Equals("electricity stock"))
                return Ok(prosumerService.GetTotalConsumptionInTheMoment(deviceCategoryName));
            else
                return BadRequest("Invalid parameter. Please use 'Electricity Producer', 'Electricity Consumer' or 'Electricity Stock'.");
        }

        /// <summary>
        /// Total Consumption/Production in The Moment (Settlement)
        /// </summary>
        [HttpGet]
        [Route("{deviceCategoryName}/{settlementName}")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetTotalConsumptionInTheMomentForCategoryInSettlement([FromRoute] string deviceCategoryName, [FromRoute] string settlementName)
        {
            if(!_sqliteDb.Settlements.Any(s => EF.Functions.Like(s.Name, $"%{settlementName}%")))
            {
                return NotFound(new { message = "Settlement with the name: " + settlementName.ToString() + " does not exist." });
            }

            if (deviceCategoryName.ToLower().Equals("electricity producer") || deviceCategoryName.ToLower().Equals("electricity consumer") | deviceCategoryName.ToLower().Equals("electricity stock"))
                return Ok(prosumerService.GetTotalConsumptionInTheMomentForSettlement(deviceCategoryName, settlementName));
            return BadRequest("Invalid parameter. Please use 'Electricity Producer', 'Electricity Consumer' or 'Electricity Stock'.");
        }

        /// <summary>
        /// Total Consumption/Production in The Moment (City)
        /// </summary>
        [HttpGet]
        [Route("{deviceCategoryName}/city/{cityName}")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetTotalConsumptionInTheMomentForCategoryInCity([FromRoute] string deviceCategoryName, [FromRoute] string cityName)
        {
            if (!_sqliteDb.Cities.Any(s => EF.Functions.Like(s.Name, $"%{cityName}%")))
            {
                return NotFound(new { message = "City with the name: " + cityName.ToString() + " does not exist." });
            }

            if (deviceCategoryName.ToLower().Equals("electricity producer") || deviceCategoryName.ToLower().Equals("electricity consumer") | deviceCategoryName.ToLower().Equals("electricity stock"))
                return Ok(prosumerService.GetTotalConsumptionInTheMomentForCity(deviceCategoryName, cityName));
            else
                return BadRequest("Invalid parameter. Please use 'Electricity Producer', 'Electricity Consumer' or 'Electricity Stock'.");
        }

        /// <summary>
        /// Total Consumption/Production in The Moment (one prosumer)
        /// </summary>
        [HttpGet]
        [Route("{deviceCategoryName}/{userId:long}")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetTotalConsumptionInTheMomentForCategoryOneProsumer([FromRoute] string deviceCategoryName, [FromRoute] long userId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
            {
                return NotFound(new { message = "User with the id: " + userId.ToString() + " does not exist." });
            }

            if (!_sqliteDb.Devices.Any(d => d.UserId == userId))
            {
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not have registered devices." });
            }

            if (!_sqliteDb.DeviceCategories.Any(dc => EF.Functions.Like(dc.Name, $"%{deviceCategoryName}%")))
                return NotFound(new { message = "Device category: " + deviceCategoryName.ToString() + " does not exist." });


            if (deviceCategoryName.ToLower().Equals("electricity producer") || deviceCategoryName.ToLower().Equals("electricity consumer") | deviceCategoryName.ToLower().Equals("electricity stock"))
                return Ok(prosumerService.GetTotalConsumptionInTheMomentForOneProsumer(deviceCategoryName, userId));
            else
                return BadRequest("Invalid parameter. Please use 'Electricity Producer', 'Electricity Consumer' or 'Electricity Stock'.");
        }

        /// <summary>
        /// Average Consumption/Production in The Moment (settlement)
        /// </summary>
        [HttpGet]
        [Route("{deviceCategoryName}/avg/{settlementName}")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetAvgConsumptionInTheMomentForSettlement([FromRoute] string deviceCategoryName, [FromRoute] string settlementName)
        {
            if (!_sqliteDb.DeviceCategories.Any(dc => EF.Functions.Like(dc.Name, $"%{deviceCategoryName}%")))
                return NotFound(new { message = "Device category: " + deviceCategoryName.ToString() + " does not exist." });


            if (deviceCategoryName.ToLower().Equals("electricity producer") || deviceCategoryName.ToLower().Equals("electricity consumer") | deviceCategoryName.ToLower().Equals("electricity stock"))
            {
                var energy = prosumerService.GetTotalConsumptionInTheMomentForSettlement(deviceCategoryName, settlementName);
                var averageEnergy = prosumerService.GetAverageConsumptionInTheMomentForSettlement(settlementName, energy);
                return Ok(averageEnergy);
            }
            else
                return BadRequest("Invalid parameter. Please use 'Electricity Producer', 'Electricity Consumer' or 'Electricity Stock'.");
        }

        /// <summary>
        /// Average Consumption/Production in The Moment (city)
        /// </summary>
        [HttpGet]
        [Route("{deviceCategoryName}/average/{cityName}")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetAvgConsumptionInTheMomentForCity([FromRoute] string deviceCategoryName, [FromRoute] string cityName)
        {
            if (!_sqliteDb.DeviceCategories.Any(dc => EF.Functions.Like(dc.Name, $"%{deviceCategoryName}%")))
                return NotFound(new { message = "Device category: " + deviceCategoryName.ToString() + " does not exist." });


            if (deviceCategoryName.ToLower().Equals("electricity producer") || deviceCategoryName.ToLower().Equals("electricity consumer") | deviceCategoryName.ToLower().Equals("electricity stock"))
            {
                var energy = prosumerService.GetTotalConsumptionInTheMomentForCity(deviceCategoryName, cityName);
                var averageEnergy = prosumerService.GetAverageConsumptionInTheMomentForCity(cityName, energy);
                return Ok(averageEnergy);
            }
            else
                return BadRequest("Invalid parameter. Please use 'Electricity Producer', 'Electricity Consumer' or 'Electricity Stock'.");
        }

        /// <summary>
        /// Average Consumption/Production in The Moment (country)
        /// </summary>
        [HttpGet]
        [Route("{deviceCategoryName}/average/")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetAvgConsumptionInTheMomentForCountry([FromRoute] string deviceCategoryName)
        {
            if (!_sqliteDb.DeviceCategories.Any(dc => EF.Functions.Like(dc.Name, $"%{deviceCategoryName}%")))
                return NotFound(new { message = "Device category: " + deviceCategoryName.ToString() + " does not exist." });


            if (deviceCategoryName.ToLower().Equals("electricity producer") || deviceCategoryName.ToLower().Equals("electricity consumer") | deviceCategoryName.ToLower().Equals("electricity stock"))
            {
                var energy = prosumerService.GetTotalConsumptionInTheMoment(deviceCategoryName);
                var averageEnergy = prosumerService.GetAverageConsumptionProductionInTheMomentForAllProsumers(energy);
                return Ok(averageEnergy);
            }
            else
                return BadRequest("Invalid parameter. Please use 'Electricity Producer', 'Electricity Consumer' or 'Electricity Stock'.");
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
    }
}
