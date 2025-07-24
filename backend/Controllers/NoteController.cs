using System.Security.Claims;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
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

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out var userId))
                return userId;
            return null;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAll()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var notes = await service.GetNotesByUserId(userId.Value);

            var dtoList = notes.Select(note => new NoteDto
            {
                NoteID = note.NoteID,
                Title = note.NoteTitle,
                Content = note.NoteContent,
                CreatedOn = note.CreatedOn,
                UpdatedOn = note.UpdatedOn
            });

            return Ok(dtoList);
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> Get(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var note = await service.GetNoteById(id);
            if (note == null || note.UserId != userId.Value)
                return NotFound("Note not found or access denied.");

            var dto = new NoteDto
            {
                NoteID = note.NoteID,
                Title = note.NoteTitle,
                Content = note.NoteContent,
                CreatedOn = note.CreatedOn,
                UpdatedOn = note.UpdatedOn
            };

            return Ok(note);
        }

        [Authorize]
        [HttpGet("by-title/{title}")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetByTitle(string title)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var notes = await service.GetNoteByTitle(title, userId.Value);

            if (!notes.Any())
                return NotFound($"No notes with the title containing '{title}' found for this user.");

            if (title.Length < 3)
                return BadRequest("Search term must be at least 3 characters.");

            var dtoList = notes.Select(note => new NoteDto
            {
                NoteID = note.NoteID,
                Title = note.NoteTitle,
                Content = note.NoteContent,
                CreatedOn = note.CreatedOn,
                UpdatedOn = note.UpdatedOn
            });

            return Ok(dtoList);
        }



        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] NoteCreateDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var note = new Note
            {
                NoteTitle = dto.NoteTitle,
                NoteContent = dto.NoteContent,
                UserId = userId.Value,
                CreatedOn = DateTime.Now
            };

            await service.AddNote(note);
            return Ok("Note added.");
        }


        [Authorize]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] NoteUpdateDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var existingNote = await service.GetNoteById(dto.NoteID);
            if (existingNote == null || existingNote.UserId != userId.Value)
                return NotFound("Note not found or access denied.");

            existingNote.NoteTitle = dto.NoteTitle;
            existingNote.NoteContent = dto.NoteContent;
            existingNote.UpdatedOn = DateTime.Now;

            var result = await service.UpdateNote(existingNote);
            if (result == 0) return NotFound("Note not found.");
            return Ok("Note updated.");
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var deleted = await service.DeleteNote(id, userId.Value);
            if (deleted == 0)
                return NotFound("Note not found or access denied.");

            return Ok("Note deleted.");
        }
    }
}
