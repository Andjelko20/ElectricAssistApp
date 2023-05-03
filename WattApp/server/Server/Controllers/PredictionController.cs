using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Route("WeekByDay/Device/{deviceId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetDevicePredictionForNextWeekByDay([FromRoute] long deviceId)
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
        [Route("WeekByDay/User/{userId:long}/{deviceCategoryId:long}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetUserPredictionForNextWeekByDay([FromRoute] long userId, [FromRoute] long deviceCategoryId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

            var PredictionForNextWeek = predictionService.UserPredictionForTheNextWeek(userId, deviceCategoryId);
            return Ok(PredictionForNextWeek);
        }

        /// <summary>
        /// City|Settlement prediction for next week (by day)
        /// </summary>
        [HttpGet]
        [Route("WeekByDay/")]
        public async Task<IActionResult> GetCityOrSettlementPredictionForNextWeekByDay([FromQuery] long cityId, long settlementId, long deviceCategoryId)
        {
            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

            if (cityId != 0)
            {
                if (!_sqliteDb.Cities.Any(c => c.Id == cityId))
                    return NotFound(new { message = "City with the ID: " + cityId.ToString() + " does not exist." });

                var PredictionForNextWeek = predictionService.CityPredictionForTheNextWeek(cityId, deviceCategoryId);
                return Ok(PredictionForNextWeek);
            }
            else //if (settlementId != 0)
            {
                if (!_sqliteDb.Settlements.Any(s => s.Id == settlementId))
                    return NotFound(new { message = "Settlement with the ID: " + settlementId.ToString() + " does not exist." });

                var PredictionForNextWeek = predictionService.SettlementPredictionForTheNextWeek(settlementId, deviceCategoryId);
                return Ok(PredictionForNextWeek);
            }
        }
    }
}
