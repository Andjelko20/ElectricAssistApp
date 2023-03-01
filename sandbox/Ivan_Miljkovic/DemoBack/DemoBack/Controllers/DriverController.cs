using DemoBack.Models;
using DemoBack.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoBack.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : Controller
    {
        private readonly DriverService _driverService;
        public DriverController(DriverService driverService) => _driverService = driverService;

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Driver>> Get(string id)
        {
            var existingDriver = await _driverService.GetAsync(id);
            if (existingDriver is null)
            {
                return NotFound();
            }
            return Ok(existingDriver);

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allDrivers = await _driverService.GetAsync();
            if (allDrivers is null)
            {
                return NotFound();
            }
            return Ok(allDrivers);

        }
        [HttpPost]
        public async Task<IActionResult> Post(Driver driver)
        {
            await _driverService.CreateAsync(driver);
            return NoContent();
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Driver driver)
        {
            var existingDriver = await _driverService.GetAsync(id);
            if (existingDriver is null)
            {
                return BadRequest();
            }
            driver.Id = existingDriver.Id;

            await _driverService.UpdateAsync(id, driver);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingDriver = await _driverService.GetAsync(id);
            if (existingDriver is null)
            {
                return BadRequest();
            }

            await _driverService.RemoveAsync(id);

            return NoContent();
        }
    }
}
