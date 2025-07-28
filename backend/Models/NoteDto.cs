namespace backend.Models;
public class NoteDto
{
    public int NoteID { get; set; }
    public string NoteTitle { get; set; } = "";
    public string? NoteContent { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
}