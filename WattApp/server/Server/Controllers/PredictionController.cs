﻿using Microsoft.AspNetCore.Mvc;
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
        /// City|Settlement prediction for next/previous week (by day)
        /// </summary>
        [HttpGet]
        [Route("WeekByDay/")]
        public async Task<IActionResult> GetCityOrSettlementPredictionForNextWeekByDay([FromQuery] long cityId, long settlementId, long deviceCategoryId,
                                                                                                   long previousCityId, long previousSettlementId, long previousProsumerId, long previousDeviceId)
        {
            if (cityId != 0)
            {
                if (!_sqliteDb.Cities.Any(c => c.Id == cityId))
                    return NotFound(new { message = "City with the ID: " + cityId.ToString() + " does not exist." });

                if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                var PredictionForNextWeek = predictionService.CityPredictionForTheNextWeek(cityId, deviceCategoryId);
                return Ok(PredictionForNextWeek);
            }
            else if (settlementId != 0)
            {
                if (!_sqliteDb.Settlements.Any(s => s.Id == settlementId))
                    return NotFound(new { message = "Settlement with the ID: " + settlementId.ToString() + " does not exist." });

                if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                var PredictionForNextWeek = predictionService.SettlementPredictionForTheNextWeek(settlementId, deviceCategoryId);
                return Ok(PredictionForNextWeek);
            }
            else if (previousCityId != 0)
            {
                if (!_sqliteDb.Cities.Any(s => s.Id == previousCityId))
                    return NotFound(new { message = "City with the ID: " + previousCityId.ToString() + " does not exist." });

                if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                var PredictionForPastWeek = predictionService.CityPredictionForThePastWeek(previousCityId, deviceCategoryId);
                return Ok(PredictionForPastWeek);
            }
            else if (previousSettlementId != 0)
            {
                if (!_sqliteDb.Settlements.Any(s => s.Id == previousSettlementId))
                    return NotFound(new { message = "Settlement with the ID: " + previousSettlementId.ToString() + " does not exist." });

                if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                var PredictionForPastWeek = predictionService.SettlementPredictionForThePastWeek(previousSettlementId, deviceCategoryId);
                return Ok(PredictionForPastWeek);
            }
            else if (previousProsumerId != 0)
            {
                if (!_sqliteDb.Users.Any(s => s.Id == previousProsumerId))
                    return NotFound(new { message = "User with the ID: " + previousProsumerId.ToString() + " does not exist." });

                if (!_sqliteDb.DeviceCategories.Any(u => u.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                var PredictionForPastWeek = predictionService.ProsumerPredictionForThePastWeek(previousProsumerId, deviceCategoryId);
                return Ok(PredictionForPastWeek);
            }
            else // if (previousDeviceId != 0)
            {
                if (!_sqliteDb.Devices.Any(d => d.Id == previousDeviceId))
                    return NotFound(new { message = "Device with the ID: " + previousDeviceId.ToString() + " does not exist." });

                var PredictionForPastWeek = predictionService.DevicePredictionForThePastWeek(previousDeviceId);
                return Ok(PredictionForPastWeek);
            }
        }
    }
}
