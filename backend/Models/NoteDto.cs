namespace backend.Models;
public class NoteDto
{
    public int NoteID { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
}