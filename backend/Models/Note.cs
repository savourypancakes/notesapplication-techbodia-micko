using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;
public class Note
{
    public int NoteID { get; set; }
    public required int UserID { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
    public required string NoteTitle { get; set; }
    public string? NoteContent { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
}