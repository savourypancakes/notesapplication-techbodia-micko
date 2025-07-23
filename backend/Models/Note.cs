namespace backend.Models;
public class Note
{
    public required int Id;
    public required string Title;
    public required string Content;
    public DateTime CreatedOn;
    public DateTime UpdatedOn;
}