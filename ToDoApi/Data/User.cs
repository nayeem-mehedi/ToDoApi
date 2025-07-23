namespace ToDoApi.Data;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = String.Empty;
    public string PasswordHash { get; set; } = String.Empty;
    public string Role { get; set; } = "USER";
}