namespace backend.Models;
public class User
{
    public int UserID { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime CreatedOn { get; set; }
}
