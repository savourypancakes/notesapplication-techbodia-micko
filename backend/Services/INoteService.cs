using backend.Models;
public interface INoteService
{
    Task<IEnumerable<Note>> GetAllNotes();
    Task<Note?> GetNoteById(int id);
    Task<int> AddNote(Note note);
    Task<int> UpdateNote(Note note);
    Task<int> DeleteNote(int id);
}