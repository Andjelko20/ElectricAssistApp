using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Model;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/notes")]
    public class NotesController:Controller
    {
        public readonly NoteDbContext context;

        public NotesController(NoteDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var notes=await context.Notes.ToListAsync();
            return Ok(notes);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var note = await context.Notes.FirstOrDefaultAsync(x=>x.Id==id);
            if(note!=null)
                return Ok(note);
            return NotFound(new { message = "Note not found" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] Note note)
        {
            note.Id = Guid.NewGuid();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x=>x.Errors.Select(y=>y.ErrorMessage)));
            }
            await context.Notes.AddAsync(note);
            await context.SaveChangesAsync();

            return Ok(new { message = "Created" });
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateNote([FromRoute]Guid id, [FromBody]Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors.Select(y => y.ErrorMessage)));
            }
            var existingNote = await context.Notes.FirstOrDefaultAsync(x => x.Id == id);
            if (existingNote != null)
            {
                existingNote.Title = note.Title;
                existingNote.Description = note.Description;
                await context.SaveChangesAsync();
                return Ok(new { message = "Updated" });
            }
            return NotFound(new { message = "Not found note with id " + id });
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteNote([FromRoute] Guid id)
        {
            var existingNote = await context.Notes.FirstOrDefaultAsync(x => x.Id == id);
            if (existingNote != null)
            {
                context.Remove(existingNote);
                await context.SaveChangesAsync();
                return Ok(new { message = "Deleted" });
            }
            return NotFound(new { message = "Not found note with id " + id });
        }
    }
}
