using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionController : Controller
    {
        public readonly SqliteDbContext _sqliteDb;
        public readonly IPredictionService predictionService;

        public PredictionController(SqliteDbContext sqliteDb, IPredictionService predictionService)
        {
            _sqliteDb = sqliteDb;
            this.predictionService = predictionService;
        }

        /// <summary>
        /// Device prediction for next week (by day)
        /// </summary>
        [HttpGet]
        [Route("WeekByDay/Device/{deviceId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetDevicePredictionForNextWeekByDay([FromRoute] int deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceEnergyUsages.Any(u => u.DeviceId == deviceId))
                return Ok(0.0); // njemu je predikcija potrosnje 0 jer nije bio ukljucen nijednom, tj. nije trosio struju

            var PredictionForNextWeek = predictionService.ConsumptionPredictionForTheNextWeek(deviceId);
            return Ok(PredictionForNextWeek);
        }
    }
}
