using backend.Models;
public interface INoteService
{
    Task<IEnumerable<Note>> GetAllNotes();
    Task<Note?> GetNoteById(int id);
    Task<IEnumerable<Note>> GetNoteByTitle(string title, int userId);
    Task<IEnumerable<Note>> GetNotesByUserId(int userId);
    Task<int> AddNote(Note note);
    Task<int> UpdateNote(Note note);
    Task<int> DeleteNote(int id, int userId);
}