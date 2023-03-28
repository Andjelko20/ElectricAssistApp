using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Server.Data;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryController : Controller
    {
        public readonly SqliteDbContext _sqliteDb;
        public readonly IHistoryService historyService;

        public HistoryController(SqliteDbContext sqliteDb, IHistoryService historyService)
        {
            _sqliteDb = sqliteDb;
            this.historyService = historyService;
        }

        [HttpGet]
        [Route("Year/{deviceId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInLastYear([FromRoute] int deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId)) // Any - da li postoji stavka tog id-a u bazi
            {
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString()  + " does not exist." }); // ako ne postoji vrati NotFound
            }

            // ukoliko postoji vrati listu svih redova
            var historyList = historyService.GetUsageHistoryForDeviceInLastYear(deviceId);
            return Ok(historyList);
        }

        [HttpGet]
        [Route("Month/{deviceId:int}")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInLastMonth([FromRoute] int deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
            {
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });
            }

            var historyList = historyService.GetUsageHistoryForDeviceInLastMonth(deviceId);
            return Ok(historyList);
        }

        [HttpGet]
        [Route("Day/{deviceId:int}")]

        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInLastDay([FromRoute] int deviceId)
        {
            if (!_sqliteDb.Devices.Any(u => u.Id == deviceId))
                return NotFound(new { message = "Device with the ID: " + deviceId.ToString() + " does not exist." });

            if (!_sqliteDb.DeviceEnergyUsages.Any(u => u.DeviceId == deviceId))
                return Ok(0.0); // jer je potrosnja 0 ako nije paljen

            var HistoryForThePreviousDay = historyService.GetUsageHistoryForDeviceInLastDay(deviceId);
            return Ok(HistoryForThePreviousDay);
        }
    }
}
