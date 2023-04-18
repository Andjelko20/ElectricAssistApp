using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Server.Data;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DSOController : Controller
    {
        public readonly SqliteDbContext _sqliteDb;
        public readonly IDSOService dsoService;

        public DSOController(SqliteDbContext sqliteDb, IDSOService dsoService)
        {
            _sqliteDb = sqliteDb;
            this.dsoService = dsoService;
        }

        /// <summary>
        /// Get CityId
        /// </summary>
        [HttpGet]
        [Route("City/")]
        //[Authorize(Roles = "dispecer, prosumer, guest")]
        public async Task<IActionResult> GetHistoryForDeviceInLastYear([FromQuery] string cityName)
        {
            var cityId = dsoService.GetCityId(cityName);

            if (cityId == -1)
                return NotFound(new { message = "City with name: " + cityName.ToString() + " does not exist." });

            return Ok(cityId);
        }
    }
}
