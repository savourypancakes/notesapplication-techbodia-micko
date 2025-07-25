namespace backend.Models;
public class NoteReadDto
{
    public int NoteID { get; set; }
    public string NoteTitle { get; set; } = null!;
    public string? NoteBody { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
