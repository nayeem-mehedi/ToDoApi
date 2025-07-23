using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Auth;
using ToDoApi.Data;
using ToDoApi.Models;

namespace ToDoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtSService;
    private readonly TodoDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthController(JwtService jwtSService, TodoDbContext dbContext,  IPasswordHasher<User> passwordHasher)
    {
        _jwtSService = jwtSService;
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        var usernameExists = await _dbContext.Users.AnyAsync(u => u.Username == registerRequest.Username);
        
        if(usernameExists)
            return BadRequest(new {message = "Username already exists"});

        var user = new User
        {
            Username = registerRequest.Username,
            Role = registerRequest.Role,
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, registerRequest.Password);
        
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        
        return Ok(new {message = "User registered successfully"});
    }
    

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);
        
        var _verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password);

        if (_verificationResult != PasswordVerificationResult.Success)
        {
            return Unauthorized(new { message = "Invalid Username or password" });
        }

        var token = _jwtSService.GenerateToken(loginRequest.Username, user.Role);
        return Ok(new { token });
    }
}

public class LoginRequest
{
    [Required(ErrorMessage = "Username is required")]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
    public string Username { get; set; } = "";
    
    [Required(ErrorMessage = "Password cannot be empty")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be 6-100 characters")]
    [NotCommonPassword(ErrorMessage = "Please choose a stronger password")]
    public string Password { get; set; } = "";
}

public class NotCommonPasswordAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var str = value as string;
        return str != "password" && str != "123456";
    }
}