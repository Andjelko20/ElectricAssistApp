using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Models.DropDowns.Location;
using Server.Services;
using System.Data;

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
        /// Total consumption/production in the moment from all prosumers
        /// </summary>
        [HttpGet]
        [Route("{ConsumptionOrProduction:regex(consumption|production)}")]
        //[Authorize(Roles = "dispecer")]
        public async Task<IActionResult> GetTotalConsumptionOrProductionForAllProsumersInTheMoment([FromRoute] string ConsumptionOrProduction)
        {
            double TotalResult = -1;

            if (ConsumptionOrProduction.ToLower() == "consumption")
            {
                TotalResult = prosumerService.GetTotalConsumptionInTheMoment();
                return Ok(TotalResult);
            }
            return BadRequest("Invalid parameter. Please use 'consumption' or 'production'.");
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
