using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class NoteUpdateDto
{
    [Required]
    public int NoteID { get; set; }

    [Required]
    public string NoteTitle { get; set; } = null!;

    public string? NoteContent { get; set; } = null!;
}
