using FullStack_Demo_API.DB;
using FullStack_Demo_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStack_Demo_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZaposleniController : Controller
    {
        private readonly FullStackDemoDbContext _fullStackDemoDbContext;

        public ZaposleniController(FullStackDemoDbContext fullStackDemoDbContext)
        {
            this._fullStackDemoDbContext = fullStackDemoDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllZaposleni()
        {
            var zaposlen = await _fullStackDemoDbContext.Zaposleni.ToListAsync();

            return Ok(zaposlen);
        }

        [HttpPost]
        public async Task<IActionResult> AddZaposleni([FromBody] Zaposlen zaposlenRequest)
        {
            zaposlenRequest.id = Guid.NewGuid(); // uvek pravimo novi id za radnika, ne veruemo angular ap/u xd

            await _fullStackDemoDbContext.Zaposleni.AddAsync(zaposlenRequest);
            await _fullStackDemoDbContext.SaveChangesAsync();

            return Ok(zaposlenRequest);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetZaposlenog([FromRoute] Guid id)
        {
            var zaposleni = await _fullStackDemoDbContext.Zaposleni.FirstOrDefaultAsync(x => x.id == id);
            
            if(zaposleni == null)
            {
                return NotFound();
            }

            return Ok(zaposleni);
        }
    }
}
