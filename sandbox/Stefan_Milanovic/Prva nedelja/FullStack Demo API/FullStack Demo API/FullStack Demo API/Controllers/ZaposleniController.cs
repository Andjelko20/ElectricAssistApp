﻿using FullStack_Demo_API.DB;
using FullStack_Demo_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateZaposlenog([FromRoute] Guid id, Zaposlen updateZaposleniRequest)
        {
            var zaposlen = await _fullStackDemoDbContext.Zaposleni.FindAsync(id);
            if(zaposlen == null)
            {
                return NotFound();
            }

            zaposlen.ime = updateZaposleniRequest.ime;
            zaposlen.email = updateZaposleniRequest.email;
            zaposlen.plata = updateZaposleniRequest.plata;
            zaposlen.telefon = updateZaposleniRequest.telefon;
            zaposlen.odeljenje = updateZaposleniRequest.odeljenje;

            await _fullStackDemoDbContext.SaveChangesAsync();

            return Ok(zaposlen);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteZaposlenog([FromRoute] Guid id)
        {
            var zaposlen = await _fullStackDemoDbContext.Zaposleni.FindAsync(id);
            if(zaposlen == null)
            {
                return NotFound();
            }

            _fullStackDemoDbContext.Zaposleni.Remove(zaposlen);
            await _fullStackDemoDbContext.SaveChangesAsync();

            return Ok(zaposlen);
        }
    }
}
