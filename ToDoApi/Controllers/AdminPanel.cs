using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Auth;
using ToDoApi.Data;
using ToDoApi.Models;

namespace ToDoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminPanel : ControllerBase
{
    private readonly TodoDbContext _dbContext;

    public AdminPanel(TodoDbContext dbContext, JwtService jwtService)
    {
        _dbContext = dbContext;
    }
    
    [Authorize(Roles = "ADMIN")]
    [HttpGet("stats")]
    public IActionResult Stats()
    {
        return Ok(new { Message = "Only Admins can Access" });
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPut("update-role")]
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleRequest updateRoleRequest)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == updateRoleRequest.Username);
        if(user == null)
            return NotFound();

        if (updateRoleRequest.Role != "")
        {
            user.Role = updateRoleRequest.Role;
            await _dbContext.SaveChangesAsync();
            
            return Ok(new { Message = "Role updated for user : "+user.Username });
        }
        
        return BadRequest(new { Message = "Error updating role. Please try again" });
    }
    
    
}