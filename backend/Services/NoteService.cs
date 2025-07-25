using Dapper;
using backend.Models;
using System.Data;

public class NoteService : INoteService
{
    private readonly IDbConnection db;

    public NoteService(IDbConnection dataBase)
    {
        db = dataBase;
    }

    public async Task<IEnumerable<Note>> GetAllNotes()
    {
        var sql = "SELECT * FROM Note";
        return await db.QueryAsync<Note>(sql);
    }
    public async Task<Note?> GetNoteById(int id)
    {
        var sql = "SELECT * FROM Note WHERE NoteID = @Id";
        return await db.QueryFirstOrDefaultAsync<Note>(sql, new { Id = id });
    }
    public async Task<IEnumerable<Note>> GetNoteByTitle(string title, int userId)
    {
        var sql = "SELECT * FROM Note WHERE NoteTitle LIKE @Title AND UserId = @UserId";
        return await db.QueryAsync<Note>(sql, new { Title = $"{title}%", UserId = userId });

    }

    public async Task<IEnumerable<Note>> GetNotesByUserId(int userId)
    {
        var sql = "SELECT * FROM Note WHERE UserId = @UserId";
        return await db.QueryAsync<Note>(sql, new { UserId = userId });
    }
    public async Task<int> AddNote(Note note)
    {
        var sql = @"
        INSERT INTO Note (NoteTitle, NoteContent, CreatedOn, UpdatedOn, UserId) 
        VALUES (@NoteTitle, @NoteContent, @CreatedOn, @UpdatedOn, @UserId)";
        return await db.ExecuteAsync(sql, note);
    }
    public async Task<int> UpdateNote(Note note)
    {
        var sql = @"
        UPDATE Note
        SET NoteTitle = @NoteTitle,
            NoteContent = @NoteContent,
            UpdatedOn = @UpdatedOn
        WHERE NoteID = @NoteID AND UserId = @UserId";
        return await db.ExecuteAsync(sql, note);
    }
    public async Task<int> DeleteNote(int id, int userId)
    {
        var sql = "DELETE FROM Note WHERE NoteID = @Id AND UserId = @UserId";
        return await db.ExecuteAsync(sql, new { Id = id, UserId = userId });
    }
}