﻿using Microsoft.AspNetCore.Mvc;
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
        [Route("WeekByDay/User/{userId:int}/{deviceCategoryId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetUserPredictionForNextWeekByDay([FromRoute] int userId, [FromRoute] int deviceCategoryId)
        {
            if (!_sqliteDb.Users.Any(u => u.Id == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not exist." });

            if (!_sqliteDb.Devices.Any(u => u.UserId == userId))
                return NotFound(new { message = "User with the ID: " + userId.ToString() + " does not have registered devices." });

            if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

            if (!_sqliteDb.Devices.Any(u => u.DeviceCategoryId == deviceCategoryId))
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });

            var PredictionForNextWeek = predictionService.UserPredictionForTheNextWeek(userId, deviceCategoryId);
            return Ok(PredictionForNextWeek);
        }
    }
}
