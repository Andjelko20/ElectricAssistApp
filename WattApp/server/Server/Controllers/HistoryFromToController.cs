﻿using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryFromToController : Controller
    {
        public readonly SqliteDbContext _sqliteDb;
        public readonly IHistoryFromToService historyFromToService;

        public HistoryFromToController(SqliteDbContext sqliteDb, IHistoryFromToService historyFromToService)
        {
            _sqliteDb = sqliteDb;
            this.historyFromToService = historyFromToService;
        }

        /// <summary>
        /// Histroy - From To
        /// </summary>
        [HttpGet]
        [Route("FromTo")]
        public async Task<IActionResult> GetHistoryForCity([FromQuery] string fromDate, string toDate, long deviceCategoryId, long cityId)
        {
            if (!_sqliteDb.Cities.Any(c => c.Id == cityId))
                return NotFound(new { message = "City with the ID: " + cityId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

            double result = historyFromToService.GetCityDoubleHistoryFromTo(fromDate, toDate, deviceCategoryId, cityId);

            return Ok(result);
        }
    }
}
