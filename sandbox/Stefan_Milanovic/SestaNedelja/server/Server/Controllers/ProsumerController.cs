using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
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
    }
}
