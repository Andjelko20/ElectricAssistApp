using Back_End.Data;
using Back_End.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly DataContext _context;
        public FilmController(DataContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Film>>> getFilms()
        {
            return Ok(await _context.film.ToListAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Film>>> getFilm(int id)
        {
            
            return Ok(await _context.film.FindAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<List<Film>>> createFilm(Film film)
        {
            _context.film.Add(film);
            await _context.SaveChangesAsync();

            return Ok(await _context.film.ToListAsync());

        }
        [HttpPut]
        public async Task<ActionResult<List<Film>>> updateFilm(Film film)
        {
            var dbFilm = await _context.film.FindAsync(film.id);
            if(dbFilm == null)
            {
                return BadRequest("Film not found");
            }
            dbFilm.name = film.description;
            dbFilm.description= film.description;
            dbFilm.genre= film.genre;
            dbFilm.actor= film.actor;
            dbFilm.iconUrl= film.iconUrl;


            await _context.SaveChangesAsync();

            return Ok(await _context.film.ToListAsync());
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Film>>> deleteFilm(int id)
        {
            var dbFilm=await _context.film.FindAsync(id);

            _context.film.Remove(dbFilm);
            await _context.SaveChangesAsync();

            return Ok(await _context.film.ToListAsync());
        }
    }
}
