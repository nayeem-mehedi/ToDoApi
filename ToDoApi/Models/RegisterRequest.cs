using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Models;

public class RegisterRequest
{
    [Required] public string Username { get; set; } = "";
    [Required] public string Password { get; set; } = "";

    public string Role { get; set; } = "USER";
}