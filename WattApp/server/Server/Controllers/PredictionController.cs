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

        /// <summary>
        /// User prediction for next week (by day)
        /// </summary>
        [HttpGet]
        [Route("WeekByDay/User/{userId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetUserPredictionForNextWeekByDay([FromRoute] int userId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            if (!_sqliteDb.Devices.Any(u => u.UserId == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not have registered devices." }); // nema prijavljen uredjaj, tako da mu je predikcija 0 - ili da vratim neki drugi status?

            var PredictionForNextWeek = predictionService.UserPredictionForTheNextWeek(userId);
            return Ok(PredictionForNextWeek);
        }
    }
}
