using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrentPeriodHistoryController : Controller
    {
        public readonly SqliteDbContext _sqliteDb;
        public readonly ICurrentPeriodHistoryService currentPeriodHistoryService;

        public CurrentPeriodHistoryController(SqliteDbContext sqliteDb, ICurrentPeriodHistoryService currentPeriodHistoryService)
        {
            _sqliteDb = sqliteDb;
            this.currentPeriodHistoryService = currentPeriodHistoryService;
        }

        /// <summary>
        /// Total device Consumption/Production for current year
        /// </summary>
        [HttpGet]
        [Route("device")]
        public async Task<IActionResult> GetHistoryForDeviceFromCurrentYear([FromQuery] long doubleYearDeviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == doubleYearDeviceId))
                return NotFound(new { message = "Device with the ID: " + doubleYearDeviceId.ToString() + " does not exist." });
            
            var result = currentPeriodHistoryService.GetUsageHistoryForDeviceFromCurrentYear(doubleYearDeviceId);
            return Ok(result);
        }
    }
}
