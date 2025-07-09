using Microsoft.AspNetCore.Mvc;
using ToDoApi.Auth;

namespace ToDoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtSService;

    public AuthController(JwtService jwtSService)
    {
        _jwtSService = jwtSService;
    }

    [HttpPost]
    public IActionResult login([FromBody] LoginRequest loginRequest)
    {
        if (UserStore.users.TryGetValue(loginRequest.Username, out var password))
            if (password == loginRequest.Password)
            {
                var token = _jwtSService.GenerateToken(loginRequest.Username);
                return Ok(new { token });
            }

        return Unauthorized();
    }
}

public class LoginRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}