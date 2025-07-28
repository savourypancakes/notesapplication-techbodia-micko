using Dapper;
using backend.Models;
using System.Data;

public class NoteService : INoteService
{
    private readonly IDbConnection db;
    public NoteService(IDbConnection conn) => db = conn;

    public async Task<IEnumerable<Note>> GetAllNotes()
    {
        return await db.QueryAsync<Note>("SELECT * FROM notes");
    }

    public async Task<Note?> GetNoteById(int id)
    {
        var sql = "SELECT * FROM notes WHERE \"NoteID\" = @NoteID";
        return await db.QueryFirstOrDefaultAsync<Note>(sql, new { NoteID = id });
    }

    public async Task<IEnumerable<Note>> GetNoteByTitle(string title, int userId)
    {
        var sql = "SELECT * FROM notes WHERE \"UserID\" = @UserID AND \"NoteTitle\" ILIKE @NoteTitle";          // ILIKE = case‑insensitive
        return await db.QueryAsync<Note>(sql, new { NoteTitle = $"{title}%", UserID = userId });
    }

    public async Task<IEnumerable<Note>> GetNotesByUserId(int userId)
    {
        return await db.QueryAsync<Note>(
            "SELECT * FROM notes WHERE \"UserID\" = @UserID",
            new { UserId = userId });
    }

    public async Task<int> AddNote(Note note)
    {
        var sql = "INSERT INTO notes (\"NoteTitle\", \"NoteContent\", \"UserID\", \"CreatedOn\")VALUES (@NoteTitle, @NoteContent, @UserID, CURRENT_TIMESTAMP) RETURNING \"NoteID\";";
        return await db.ExecuteScalarAsync<int>(sql, note);  // new NoteID
    }

    public async Task<int> UpdateNote(Note note)
    {
        var sql = "UPDATE notes SET \"NoteTitle\" = @NoteTitle, \"NoteContent\" = @NoteContent, \"UpdatedOn\" = CURRENT_TIMESTAMP WHERE \"NoteID\" = @NoteID AND \"UserID\" = @UserID;";
        return await db.ExecuteAsync(sql, note);
    }

    public async Task<int> DeleteNote(int id, int userId)
    {
        var sql = "DELETE FROM notes WHERE \"NoteID\" = @NoteID AND \"UserID\" = @UserID";
        return await db.ExecuteAsync(sql, new { NoteID = id, UserID = userId });
    }
}
