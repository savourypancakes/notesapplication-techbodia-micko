namespace backend.Models;
public class Note
{
    public required int NoteID { get; set; }
    public required string NoteTitle { get; set; }
    public required string NoteContent { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
}