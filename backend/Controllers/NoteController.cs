using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly NoteService service;
        public NoteController(NoteService s)
        {
            service = s;
        }
        [HttpGet]
        public async Task<IEnumerable<Note>> GetAll() => await service.GetAllNotes();
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> Get(int id)
        {
            var note = await service.GetNoteById(id);
            if (note == null) return NotFound();
            return note;
        }
        [HttpGet("by-title/title")]
        public async Task<ActionResult<Note>> Get(string title)
        {
            var note = await service.GetNoteByTitle(title);
            if (note == null) return NotFound($"No note with the title of '{title}' was found.");
            return Ok(note);
        }
        [HttpPost]
        public async Task<ActionResult> Create(Note note)
        {
            note.CreatedOn = DateTime.Now;
            await service.AddNote(note);
            return Ok("Note added.");
        }
        [HttpPut]
        public async Task<ActionResult> Update(Note note)
        {
            if (note == null || note.NoteID < 0)
            {
                return BadRequest("Invalid note data.");
            }
            note.UpdatedOn = DateTime.Now;
            var result = await service.UpdateNote(note);
            if (result == 0) {
                return NotFound("Note not found.");
            }
            return Ok("Note updated.");
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Note>> Delete(int id)
        {
            await service.DeleteNote(id);
            return Ok("Note Deleted.");
        }
    }
}